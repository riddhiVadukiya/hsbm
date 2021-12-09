using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HSBM.EntityModel.PaymentGetWay
{
    public class SagePayRequest
    {
        public string VPSProtocol
        {
            get { return "3.0"; }
        }

        /// <summary>
        /// "PAYMENT", "DEFERRED", "AUTHENTICATE"
        /// </summary>
        public string TxType
        {
            get { return "PAYMENT"; }
        }

        /// <summary>
        /// VendorName. For get VendorName, You will need to register into SagePay Site
        /// </summary>
        public string Vendor
        {
            get { return "poboxesltd"; }
        }

        /// <summary>
        /// For Unique Transaction. Use for every request and response and store it into db for store transaction detail
        /// </summary>
        public string VendorTxCode { get; set; }

        [Format("f2")]
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Description
        {
            get { return "Test SagePay Description"; }
        }

        /// <summary>
        /// Redirect URL; Notes: This URL Must be hosted as Domain as public. And this URL not support IP Address. So must be given host url e.g. www.contonse.com
        /// </summary>
        [Unencoded]
        public string NotificationURL { get; set; }

        public string BillingSurname
        {
            get { return "Test bSurname"; }
        }

        public string BillingFirstnames
        {
            get { return "Test bFirstnames"; }
        }

        public string BillingAddress1
        {
            get { return "Test bAddress1"; }
        }

        //[Optional]
        //public string BillingAddress2 { get; set; }

        public string BillingCity
        {
            get { return "Berat"; }
        }

        public string BillingPostCode
        {
            get { return "123456"; }
        }

        /// <summary>
        /// ISO 3166 Country Codes. For Example: Albania=AL, Algeria=DZ, American Samoa=AS
        /// </summary>
        public string BillingCountry
        {
            get { return "AL"; }
        }

        //[Optional]
        //public string BillingState { get; set; }

        //[Optional]
        //public string BillingPhone { get; set; }

        public string DeliverySurname
        {
            get { return "Test dSurname"; }
        }

        public string DeliveryFirstnames
        {
            get { return "Test dFirstnames"; }
        }

        public string DeliveryAddress1
        {
            get { return "Test dAddress1"; }
        }

        //[Optional]
        //public string DeliveryAddress2 { get; set; }

        public string DeliveryCity
        {
            get { return "Berat"; }
        }

        public string DeliveryPostCode
        {
            get { return "123456"; }
        }

        public string DeliveryCountry
        {
            get { return "AL"; }
        }

        //[Optional]
        //public string DeliveryState { get; set; }

        //[Optional]
        //public string DeliveryPhone { get; set; }

        public string CustomerEMail
        {
            get { return "test@gmail.com"; }
        }

        public string Basket { get; set; }


        /**************************NOTE: Not currently supported*********************************/

        /// <summary>
        /// //For charities registered for Gift Aid, set to 1 to display the Gift Aid check box on the payment pages, or else 0
        /// </summary>
        public int AllowGiftAid
        {
            get { return 0; }
        }

        /// <summary>
        /// 0 = If 3D-Secure checks are possible and rules allow, perform the checks and apply the authorisation rules. (default)
        /// 1 = Force 3D-Secure checks for this transaction if possible and apply rules for authorisation.
        /// 2 = Do not perform 3D-Secure checks for this transaction and always authorise.
        /// 3 = Force 3D-Secure checks for this transaction if possible but ALWAYS obtain an auth code, irrespective of rule base.
        /// </summary>
        public int Apply3DSecure
        {
            get { return 1; }
        }

        /// <summary>
        /// NORMAL Or LOW
        /// </summary>
        public string Profile
        {
            get { return "NORMAL"; }
        }

        /// <summary>
        /// MerchantAccountType: Ecommerce or MailOrder. Default NULL
        /// </summary>
        public string AccountType
        {
            get { return null; }//MerchantAccountType.Ecommerce.ToString(); }
        }

        /// <summary>
        /// Sends some data to a URL using an HTTP POST.
        /// </summary>
        /// <param name="url">Url to send to</param>
        /// <param name="postData">The data to send</param>
        public string SendRequest(string url, string postData)
        {
            var uri = new Uri(url);
            var request = WebRequest.Create(uri);
            var encoding = new UTF8Encoding();
            var requestData = encoding.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Timeout = (300 * 1000); //TODO: Move timeout to config
            request.ContentLength = requestData.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(requestData, 0, requestData.Length);
            }

            var response = request.GetResponse();

            string result;

            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }

    /// <summary>
    /// Specifies that a property should not be URL Encoded when being serialized by the HttpPostSerialzier
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UnencodedAttribute : Attribute
    {
    }

    /// <summary>
    /// Specifies a format to use when serializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatAttribute : Attribute
    {
        public string Format { get; private set; }

        public FormatAttribute(string format)
        {
            Format = format;
        }
    }

    public class HttpPostSerializer
    {
        /// <summary>
        /// Serializes an object to a format usable for an HTTP POST. 
        /// All public instance properties are serialized. 
        /// </summary>
        public string Serialize(object toSerialize)
        {
            var type = toSerialize.GetType();
            var pairs = new Dictionary<string, string>();

            foreach (var property in GetProperties(type))
            {
                if (!property.CanRead) continue;

                var rawValue = property.GetValue(toSerialize, null);
                if (rawValue == null && IsOptional(property)) continue;

                var format = GetFormat(property);
                // Always use EN-GB
                string convertedValue = string.Format(CultureInfo.InvariantCulture, format, rawValue);

                if (ShouldEncode(property))
                {
                    convertedValue = HttpUtility.UrlEncode(convertedValue, System.Text.Encoding.GetEncoding("ISO-8859-15"));
                }

                pairs.Add(property.Name, convertedValue);
            }

            var result = from pair in pairs
                         select pair.Key + "=" + pair.Value;

            return string.Join("&", result.ToArray());
        }

        static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
        }

        static string GetFormat(PropertyInfo property)
        {
            var attribute = (FormatAttribute)Attribute.GetCustomAttribute(property, typeof(FormatAttribute));

            if (attribute != null)
            {
                return "{0:" + attribute.Format + "}";
            }

            return "{0}";
        }

        static bool ShouldEncode(PropertyInfo property)
        {
            return !Attribute.IsDefined(property, typeof(UnencodedAttribute));
        }

        static bool IsOptional(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(OptionalAttribute));
        }
    }

    public class TransactionRegistrationResponse
    {
        /// <summary>
        /// Protocol version
        /// </summary>
        public string VPSProtocol { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public SagepatResponseType Status { get; set; }

        /// <summary>
        /// Additional status details
        /// </summary>
        public string StatusDetail { get; set; }

        /// <summary>
        /// Transaction ID generated by SagePay
        /// </summary>
        public string VPSTxId { get; set; }

        /// <summary>
        /// Security Key
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// Redirect URL
        /// </summary>
        public string NextURL { get; set; }
    }

    public class ResponseSerializer
    {
        /// <summary>
        /// Deserializes the response into an instance of type T.
        /// </summary>
        public void Deserialize<T>(string input, T objectToDeserializeInto)
        {
            Deserialize(typeof(T), input, objectToDeserializeInto);
        }

        /// <summary>
        /// Deserializes the response into an object of type T.
        /// </summary>
        public T Deserialize<T>(string input) where T : new()
        {
            var instance = new T();
            Deserialize(typeof(T), input, instance);
            return instance;
        }

        /// <summary>
        /// Deserializes the response into an object of the specified type.
        /// </summary>
        public void Deserialize(Type type, string input, object objectToDeserializeInto)
        {
            if (string.IsNullOrEmpty(input)) return;

            var bits = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var nameValuePairCombined in bits)
            {
                int index = nameValuePairCombined.IndexOf('=');
                string name = nameValuePairCombined.Substring(0, index);
                string value = nameValuePairCombined.Substring(index + 1);

                var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

                if (prop == null)
                {
                    throw new InvalidOperationException(string.Format("Could not find a property on Type '{0}' named '{1}'", type.Name,
                                                                      name));
                }

                //TODO: Investigate building a method of defining custom serializers

                object convertedValue;

                if (prop.PropertyType == typeof(SagepatResponseType))
                {
                    convertedValue = ConvertStringToSagePayResponseType(value);
                }
                else
                {
                    convertedValue = Convert.ChangeType(value, prop.PropertyType);
                }

                prop.SetValue(objectToDeserializeInto, convertedValue, null);
            }
        }

        /// <summary>
        /// Deserializes the response into an object of the specified type.
        /// </summary>
        public object Deserialize(Type type, string input)
        {
            var instance = Activator.CreateInstance(type);
            Deserialize(type, input, instance);
            return instance;
        }

        /// <summary>
        /// Utility method for converting a string into a ResponseType. 
        /// </summary>
        public static SagepatResponseType ConvertStringToSagePayResponseType(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.StartsWith("OK"))
                {
                    return SagepatResponseType.Ok;
                }

                if (input.StartsWith("NOTAUTHED"))
                {
                    return SagepatResponseType.NotAuthed;
                }

                if (input.StartsWith("ABORT"))
                {
                    return SagepatResponseType.Abort;
                }

                if (input.StartsWith("REJECTED"))
                {
                    return SagepatResponseType.Rejected;
                }

                if (input.StartsWith("MALFORMED"))
                {
                    return SagepatResponseType.Malformed;
                }

                if (input.StartsWith("AUTHENTICATED"))
                {
                    return SagepatResponseType.Authenticated;
                }

                if (input.StartsWith("INVALID"))
                {
                    return SagepatResponseType.Invalid;
                }

                if (input.StartsWith("REGISTERED"))
                {
                    return SagepatResponseType.Registered;
                }

                if (input.StartsWith("ERROR"))
                {
                    return SagepatResponseType.Error;
                }
            }
            return SagepatResponseType.Unknown;
        }
        
    }

    #region SagePay Response Class
    public class SagepayResponseEntity
    {
        public string VPSProtocol { get; set; }

        public string TxType { get; set; }

        public string VendorTxCode { get; set; }

        public string Status { get; set; }

        public string StatusDetail { get; set; }

        public string TxAuthNo { get; set; }

        public string AVSCV2 { get; set; }

        public string AddressResult { get; set; }

        public string PostCodeResult { get; set; }

        public string CV2Result { get; set; }

        public int GiftAid { get; set; }

        public string ThreeDSecureStatus { get; set; }

        public string CAVV { get; set; }

        public string AddressStatus { get; set; }

        public string PayerStatus { get; set; }

        public string CardType { get; set; }

        public string Last4Digits { get; set; }

        public string VPSSignature { get; set; }

        public string FraudResponse { get; set; }

        public decimal? Surcharge { get; set; }

        public string DeclineCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string BankAuthCode { get; set; }

        public string Token { get; set; }

        ///////////////////Old//////////////
        //public string Status { get; set; }

        //public string TransactionStatus { get; set; }

        //public string VendorTxCode { get; set; }

        //public string BankAuthCode { get; set; }

        //public string StatusDetail { get; set; }

        //public decimal? Surcharge { get; set; }

        //public string Token { get; set; }

        //public string TxAuthNo { get; set; }

        //public string VpsTxId { get; set; }

        //public string PayerStatus { get; set; }

        //public string TransactionType { get; set; }

        //public string ThreeDSecureStatus { get; set; }

        //public string Cavv { get; set; }
    }
    #endregion

    public enum SagepatResponseType
    {
        Unknown,
        Ok,
        NotAuthed,
        Abort,
        Rejected,
        Authenticated,
        Registered,
        Malformed,
        Error,
        Invalid,
    }
}
