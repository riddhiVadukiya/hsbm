using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using HSBM.Common.Enums;
using System.Data;
namespace HSBM.Common.Utils
{
    public static class Helper
    {
        #region Variable
        readonly static Logging.ILogger _ILogger = Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        /// <summary>
        /// Comman date formate
        /// </summary>
        /// <returns></returns>
        public static string DefaultDateFormat()
        {
            try
            {
                return ConfigurationManager.AppSettings["DefaultDateFormats"].ToString();
            }
            catch (Exception)
            {

                return "dd/MM/yyyy";
            }
        }

        public static string DefaultDateFormatsForDatePicker()
        {
            try
            {
                return ConfigurationManager.AppSettings["DefaultDateFormatsForDatePicker"].ToString();
            }
            catch (Exception)
            {

                return "dd/mm/yy";
            }
        }



        #region Encrypt / Decrypt
        /// <summary>
        /// To encrypt the Tokenkey
        /// </summary>
        /// <param name="p_Tokenkey"></param>
        /// <returns>It returns encrypted code</returns>
        public static string Encrypt(string plainText)
        {
            string secretKey = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AESSecretKey"]);
            string IV = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AESIV"]);
            CryptLib _crypt = new CryptLib();
            // String iv = CryptLib.GenerateRandomIV(16); //16 bytes = 128 bits
            string key = CryptLib.getHashSha256(secretKey, 32); //32 bytes = 256 bits
            string cypherText = _crypt.encrypt(plainText, key, IV);
            cypherText = cypherText.Replace("+", "%20");
            return cypherText;
        }
        /// <summary>
        /// To Decrypt Tokenkey
        /// </summary>
        /// <param name="p_Tokenkey"></param>
        /// <returns>It returns plain Tokenkey</returns>
        public static string Decrypt(string cypherText)
        {
            cypherText = cypherText.Replace("%20", "+");
            cypherText = cypherText.Replace(" ", "+");
            string secretKey = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AESSecretKey"]);
            string IV = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AESIV"]);
            CryptLib _crypt = new CryptLib();
            // String iv = CryptLib.GenerateRandomIV(16); //16 bytes = 128 bits
            string key = CryptLib.getHashSha256(secretKey, 32); //32 bytes = 256 bits
            string plainText = _crypt.decrypt(cypherText, key, IV);
            return plainText;
        }
        #endregion

        #region EnumDescription

        /// <summary>
        /// Get enum description.
        /// </summary>
        /// <param name="p_Value">Accepts enum value.</param>
        /// <returns>string.</returns>
        public static string GetEnumDescription(Enum p_value)
        {
            FieldInfo fi = p_value.GetType().GetField(p_value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return p_value.ToString();
        }
        public static string GetEnumTextValue(this Enum value)
        {
            var type = value.GetType();

            string name = Enum.GetName(type, value);
            if (name == null) { return null; }

            var field = type.GetField(name);
            if (field == null) { return null; }

            var attr = Attribute.GetCustomAttribute(field, typeof(TextValueAttribute)) as TextValueAttribute;
            if (attr == null) { return null; }

            return attr.Text;
        }
        public static T GetValueFromDescription<T>(string p_description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == p_description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == p_description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        #endregion

        /// <summary>
        /// truncate string 
        /// </summary>
        /// <param name="p_String">The requested String</param>
        /// <param name="p_Length">The requested Length</param>
        /// <returns>string</returns>
        /// 

        public static string ShortString(string p_String, int p_Length)
        {
            if ((!string.IsNullOrWhiteSpace(p_String)) && p_String.Length > p_Length)
            {
                return p_String.Substring(0, p_Length);
            }
            else
            {
                return p_String;
            }
        }

        public static Boolean SendMail(string p_ToEmail, string p_Subject, string p_Body, string p_BCC, string p_CC)
        {
            try
            {
                SmtpSection _SmtpSection = (SmtpSection)ConfigurationManager.GetSection("mailSettings/smtp");
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.To.Add(new MailAddress(p_ToEmail));
                _MailMessage.Body = p_Body;
                _MailMessage.IsBodyHtml = true;
                _MailMessage.Subject = p_Subject;
                _MailMessage.From = new MailAddress(_SmtpSection.From);
                if (!string.IsNullOrEmpty(p_BCC))
                {
                    string[] _BccEmails = p_BCC.Split(';');
                    foreach (var _Email in _BccEmails)
                    {
                        _MailMessage.Bcc.Add(_Email);
                    }
                }
                if (!string.IsNullOrEmpty(p_CC))
                {
                    string[] _CcEmails = p_CC.Split(';');
                    foreach (var _Email in _CcEmails)
                    {
                        _MailMessage.CC.Add(_Email);
                    }
                }
                SmtpClient _SmtpClient = new SmtpClient();
                _SmtpClient.Host = _SmtpSection.Network.Host;
                _SmtpClient.Port = _SmtpSection.Network.Port;
                _SmtpClient.EnableSsl = _SmtpSection.Network.EnableSsl;
                _SmtpClient.Credentials = new NetworkCredential(_SmtpSection.Network.UserName, _SmtpSection.Network.Password);

                _SmtpClient.Send(_MailMessage);
                return true;
            }
            catch (Exception _Exception)
            {
                _ILogger.Error("SendMail", _Exception);
                return false;
            }
        }

        public static Boolean SendMails(List<string> p_ToMailIds, string p_Subject, string p_Body, string p_FromEmail, string p_FromName, List<string> p_ListofAttachments)
        {
            try
            {

                System.Text.StringBuilder _StringBuilder = new System.Text.StringBuilder();

                foreach (string EmailId in p_ToMailIds)
                {
                    _StringBuilder.Append("{" + "\"email\"" + ":" + "\"" + EmailId + "\"" + "," + "\"name\"" + ":" + "\"" + EmailId + "\"" + "," + "\"type\"" + ":" + "" + "\"to\"" + "},");
                }

                if (p_ListofAttachments != null)
                {

                    //string fileName = Path.GetFileName(p_ListoAttachments).ToList();
                    // _listosattachments.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                }

                string _JsonOfEmailIds = _StringBuilder.ToString().Substring(0, _StringBuilder.ToString().Length - 1);

                string _PasswordApiKey = System.Web.Configuration.WebConfigurationManager.AppSettings["SemdEmailTempleteUserKey"].ToString();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(System.Web.Configuration.WebConfigurationManager.AppSettings["MandrillApiUrlForSendMail"].ToString());
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    // This is for Live site API call
                    string json = string.Format(@"{{""key"": ""{0}"",""message"": {{""html"": ""{1}"",""subject"": ""{2}"",""from_email"": ""{3}"",""from_name"": ""{4}"",""to"": [{5}],""important"": false}},""async"": false,""ip_pool"": ""Main Pool""}}"
                      , _PasswordApiKey, "" + p_Body + "", p_Subject, p_FromEmail, p_FromName, _JsonOfEmailIds);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                return true;
            }
            catch (Exception _Exception)
            {
                _ILogger.Error("SendMail", _Exception);
                return false;
            }
        }

        public static string GetSendMailTemplate(string p_MailTemplateName)
        {
            try
            {
                StreamReader _StreamReader;
                var appDomain = System.AppDomain.CurrentDomain;
                var basePath = appDomain.BaseDirectory;
                _StreamReader = System.IO.File.OpenText(Path.Combine(basePath, "MailTemplate", p_MailTemplateName + ".html"));
                string _Body = _StreamReader.ReadToEnd();
                _StreamReader.Close();
                return _Body;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string GenerateRandomNumber()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(100000, 999999); // creates a 8 digit random no.

            return myRandomNo.ToString();
        }

        static public string GenerateRandomAlphaNumeric(int NoOfCharacter)
        {
            string randomAlphaNumeric = String.Empty;
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
                var random = new Random();
                randomAlphaNumeric = new string(
                    Enumerable.Repeat(chars, NoOfCharacter)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());

                return randomAlphaNumeric;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string SaveImageFromBase64(string fullImagePath, string base64)
        {
            string _fileName = string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(FixBase64ForImage(base64));

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image image;
                    image = Image.FromStream(ms, true);

                    if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                    {
                        _fileName = Guid.NewGuid() + ".jpg";
                    }
                    else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                    {
                        _fileName = Guid.NewGuid() + ".gif";
                    }
                    else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                    {
                        _fileName = Guid.NewGuid() + ".png";
                    }
                    else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                    {
                        _fileName = Guid.NewGuid() + ".bmp";
                    }

                    fullImagePath += _fileName;
                    image.Save(fullImagePath);
                }
            }
            catch (Exception)
            {
            }
            return _fileName;
        }

        public static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        public static string GetUserCode(int length)
        {
            var chars = "0123456789"; //"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            T element;

            for (int i = 0; i < collection.Count; i++)
            {
                element = collection.ElementAt(i);
                if (predicate(element))
                {
                    collection.Remove(element);
                    i--;
                }
            }
        }

        public static string ReplaceHTMLTagsLineBreaks(string p_Text)
        {
            p_Text = Regex.Replace(p_Text, @"<[^>]+>|&nbsp;", "").Trim();
            return p_Text.Replace("\n", "</br>").Replace("\r\n", "</br>");
        }

        public static bool CompareList<T>(this List<T> list1, List<T> list2, List<string> skipColumns = null)
        {
            List<string> _skipColumns = new List<string>();
            _skipColumns.Add("ExtensionData");

            if (skipColumns != null && skipColumns.Count > 0)
            {
                _skipColumns.AddRange(skipColumns);
            }

            //if any of the list is null, return false
            if ((list1 == null && list2 != null) || (list2 == null && list1 != null))
                return false;
            //if both lists are null, return true, since its same
            else if (list1 == null && list2 == null)
                return true;
            //if count don't match between 2 lists, then return false
            if (list1.Count != list2.Count)
                return false;
            bool IsEqual = true;
            foreach (T item in list1)
            {
                T Object1 = item;
                T Object2 = list2.ElementAt(list1.IndexOf(item));
                Type type = typeof(T);
                //if any of the object inside list is null and other list has some value for the same object  then return false
                if ((Object1 == null && Object2 != null) || (Object2 == null && Object1 != null))
                {
                    IsEqual = false;
                    break;
                }

                foreach (System.Reflection.PropertyInfo property in type.GetProperties())
                {
                    if (!_skipColumns.Contains(property.Name))
                    {
                        string Object1Value = string.Empty;
                        string Object2Value = string.Empty;
                        if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                            Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                        if (type.GetProperty(property.Name).GetValue(Object2, null) != null)
                            Object2Value = type.GetProperty(property.Name).GetValue(Object2, null).ToString();
                        //if any of the property value inside an object in the list didnt match, return false
                        if (Object1Value.Trim() != Object2Value.Trim())
                        {
                            IsEqual = false;
                            break;
                        }
                    }
                }

            }
            //if all the properties are same then return true
            return IsEqual;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static List<string> ExtractEmailsFromString(string content)
        {
            List<string> result = new List<string>();
            try
            {
                //instantiate with this pattern 
                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                    RegexOptions.IgnoreCase);
                //find items that matches with our pattern
                MatchCollection emailMatches = emailRegex.Matches(content);

                StringBuilder sb = new StringBuilder();

                foreach (Match emailMatch in emailMatches)
                {
                    result.Add(emailMatch.Value);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }






        public static string DefaultImageLocation
        {
            get
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DefaultImageLocation"]);
            }
        }

        /// <summary>
        /// Returns Lat-Long of a location
        /// </summary>
        /// <param name="location">e.g. Rajkot</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLatitudeLongitude(string location)
        {
            try
            {
                Dictionary<string, string> _LatLong = new Dictionary<string, string>();

                string _url = "http://maps.google.com/maps/api/geocode/xml?address=" + location + "&sensor=false";
                WebRequest request = WebRequest.Create(_url);
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        DataSet dsResult = new DataSet();
                        dsResult.ReadXml(reader);
                        DataTable dtCoordinates = new DataTable();
                        dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
                        if (dsResult.Tables["result"] != null && dsResult.Tables["result"].Rows != null)
                        {
                            foreach (DataRow row in dsResult.Tables["result"].Rows)
                            {
                                string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                DataRow loca = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];

                                _LatLong.Add(loca["lat"].ToString(), loca["lng"].ToString());
                                break;
                            }
                            return _LatLong;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public static string GetCurrentCurrency()
        {
            try
            {
                HttpCookie _cookie = HttpContext.Current.Request.Cookies["Currency"];
                if (_cookie != null)
                {
                    return _cookie.Value;
                }
            }
            catch (Exception)
            {

            }
            return "";
        }

        public static string GetCurrentCurrencySymbol()
        {
            try
            {
                string cc = GetCurrentCurrency();

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("USD", "$");
                dict.Add("INR", "₹");
                dict.Add("AED", "AED");
                dict.Add("EUR", "€");
                dict.Add("GBP", "£");
                dict.Add("AUD", "AUD");
                dict.Add("ZAR", "R");

                var cs = dict.Where(t => t.Key == cc).FirstOrDefault();

                return cs.Value;

            }
            catch (Exception)
            {

            }
            return "";
        }

        public static string PaymentKey()
        {
            try
            {
                return ConfigurationManager.AppSettings["PaymentKey"].ToString();
            }
            catch (Exception)
            {

                return "";
            }
        }
        public static string PaymentSalt()
        {
            try
            {
                return ConfigurationManager.AppSettings["PaymentSalt"].ToString();
            }
            catch (Exception)
            {

                return "";
            }
        }
    }
}
