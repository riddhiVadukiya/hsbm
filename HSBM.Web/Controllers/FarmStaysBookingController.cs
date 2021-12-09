using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.EmailTemplates;
using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HSBM.EntityModel.AxisRooms;

namespace HSBM.Web.Controllers
{
    public class FarmStaysBookingController : BaseController
    {
        FrontFarmStaysBookingService _FrontFarmStaysBookingService = new FrontFarmStaysBookingService();
        EmailTemplateService _EmailTemplateService = new EmailTemplateService();
        // GET: FarmStayBooking
        //public ActionResult Index(int FarmStayId, int RoomId, string CheckIn, string CheckOut, int Guests, bool IsSolo = false)
        public ActionResult Index(string Request)
        {
            if (!string.IsNullOrEmpty(Request))
            {
                string _URL = Helper.Decrypt(Request);
                List<string> _ListofString = _URL.Split('&').ToList();
                if (_ListofString.Count() == 9)
                {
                    DateTime _BookTime = Convert.ToDateTime(_ListofString[5].Split('=')[1]);

                    int _Hours = (DateTime.Now - _BookTime).Hours;

                    if (_Hours <= 1)
                    {

                        ViewBag.FarmStayId = _ListofString[0].Split('=')[1];
                        ViewBag.RoomId = _ListofString[1].Split('=')[1];
                        ViewBag.CheckIn = _ListofString[2].Split('=')[1];
                        ViewBag.CheckOut = _ListofString[3].Split('=')[1];
                        ViewBag.Guests = _ListofString[4].Split('=')[1];
                        ViewBag.Child = _ListofString[6].Split('=')[1];
                        ViewBag.RatePlanId = _ListofString[7].Split('=')[1];
                        ViewBag.IsSolo = _ListofString[8].Split('=')[1];
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "FarmStaysHome");
            }

            return View();
        }

        public ActionResult ThankYou(string Id)
        {
            FrontOrdersMaster ordersMaster=new FrontOrdersMaster();
            try
            {
                string CurrencyCode = Helper.GetCurrentCurrency();
                string _URL = Helper.Decrypt(Id);
                List<string> _ListofString = _URL.Split('&').ToList();
                DateTime _BookTime = Convert.ToDateTime(_ListofString[1]);

                int _Hours = (DateTime.Now - _BookTime).Hours;

                if (_Hours <= 1)
                {
                    FrontOrdersService _OrdersService = new FrontOrdersService();
                    long id = Convert.ToInt64(_ListofString[0]);
                    ordersMaster = _OrdersService.GetOrderDetailByKey(id, CurrencyCode);
                    return View(ordersMaster);
                }
                else
                {
                    return RedirectToAction("Index", "FarmStaysHome");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetRoomBookingDetail(SearchFarmStaysRequest SearchFarmStaysRequest)
        {
            SearchFarmStaysRequest.CurrencyCode = Helper.GetCurrentCurrency();
            BookingResponse _FarmStaysDetail = new BookingResponse();
            _FarmStaysDetail = _FrontFarmStaysBookingService.GetRoomBookingDetail(SearchFarmStaysRequest);
            if (_FarmStaysDetail != null)
            {
                _FarmStaysDetail.FarmStayId = SearchFarmStaysRequest.FarmStayId;
                _FarmStaysDetail.RoomId = SearchFarmStaysRequest.RoomId;
                _FarmStaysDetail.CheckOut = SearchFarmStaysRequest.CheckOut;
                _FarmStaysDetail.CheckIn = SearchFarmStaysRequest.CheckIn;
                _FarmStaysDetail.Guests = SearchFarmStaysRequest.Guests;
                _FarmStaysDetail.Child = SearchFarmStaysRequest.Child;
                _FarmStaysDetail.RatePlanId = SearchFarmStaysRequest.RatePlanId;
                _FarmStaysDetail.IsSolo = SearchFarmStaysRequest.IsSolo;
                _FarmStaysDetail.CurrencyCode = SearchFarmStaysRequest.CurrencyCode;

                _FarmStaysDetail.LeadTraveler = new LeadTraveler();
                if (SessionProxy.CustomerDetails != null)
                {
                    _FarmStaysDetail.LeadTraveler.GuestFirstName = SessionProxy.CustomerDetails.FirstName;
                    _FarmStaysDetail.LeadTraveler.GuestLastName = SessionProxy.CustomerDetails.LastName;
                    _FarmStaysDetail.LeadTraveler.GuestEmail = SessionProxy.CustomerDetails.Email;
                    _FarmStaysDetail.LeadTraveler.GuestMobile = SessionProxy.CustomerDetails.Mobile;
                    _FarmStaysDetail.LeadTraveler.GuestCountryId = SessionProxy.CustomerDetails.CountryMasterID.HasValue ? (int)SessionProxy.CustomerDetails.CountryMasterID.Value : 0;
                    _FarmStaysDetail.LeadTraveler.GuestCity = SessionProxy.CustomerDetails.CityName;
                    _FarmStaysDetail.LeadTraveler.GuestAddress = SessionProxy.CustomerDetails.Address;
                    if (!string.IsNullOrEmpty(SessionProxy.CustomerDetails.Gender))
                    {
                        _FarmStaysDetail.LeadTraveler.IsMale = SessionProxy.CustomerDetails.Gender == "Male" ? true : false;
                    }

                }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(_FarmStaysDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateInput(false)]
        public ActionResult BookFarmStayRoom(BookingResponse BookingResponse)
        {
            if (BookingResponse != null)
            {
            //    string _CancellationLink = string.Empty;
                if (SessionProxy.CustomerDetails != null)
                    BookingResponse.LeadTraveler.CustomerId = SessionProxy.CustomerDetails.Id;


                long _orderId = _FrontFarmStaysBookingService.BookFarmStayRoom(BookingResponse);
                //if (_orderId > 0)
                //{
                    //if (SessionProxy.CustomerDetails == null)
                    //{
                    //    string _Url = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"Orders/ViewGuestOrderDetail?OId=" + Helper.Encrypt(_orderId.ToString());
                    //    _CancellationLink = "For View or cancel your order <a href=\"" + _Url + "\">Click Hear</a>.";
                    //}
                    //string CurrencyCode = Helper.GetCurrentCurrency();
                    //FrontOrdersService _OrdersService = new FrontOrdersService();
                    //FrontOrdersMaster ordersMaster = _OrdersService.GetOrderDetailByKey(_orderId, CurrencyCode);
                    //EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.BookingConfirmation);
                    //if (_EmailTemplates != null)
                    //{
                    //    string _CancellationPolicy = (BookingResponse.CancellationPolicyIsNonRefundable || !BookingResponse.IsApplyCancellationPolicy) ? "Non Refundable" : (BookingResponse.RefundablePercentage + "% Refund before " + BookingResponse.RefundableBeforDays + " Days");
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", BookingResponse.LeadTraveler.GuestFirstName);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#FarmStayName#", BookingResponse.FarmStayName);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Name#", BookingResponse.Name);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Type#", BookingResponse.TypeName);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckIn#", BookingResponse.CheckIn);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckOut#", BookingResponse.CheckOut);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Guests#", BookingResponse.Guests.ToString());
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Address#", BookingResponse.Address);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Price#", "₹" + ordersMaster.NetAmount.ToString());
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CancellationPolicy#", _CancellationPolicy);
                    //    _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CancellationLink#", _CancellationLink);
                    //    bool resultMail = Helper.SendMail(BookingResponse.LeadTraveler.GuestEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                    //}

                   
               // }
                return Json(_orderId, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderCancel(int Id, decimal RefundAmount = 0, string CancellationReason="")
        {
            if (Id > 0 && !string.IsNullOrEmpty(CancellationReason))
            {
                string paymentId = _FrontFarmStaysBookingService.GetPaymentIdbyOrderId(Id);

                if (!string.IsNullOrEmpty(paymentId))
                {

                    //string URI = ConfigurationManager.AppSettings["PaymentURL"].ToString() + "payment/payment/chkMerchantTxnStatus?";
                    //string myParameters = "merchantKey=" + ConfigurationManager.AppSettings["PaymentKey"].ToString() + "&paymentId=" + paymentId + "&refundAmount=" + RefundAmount;


                    //using (WebClient wc = new WebClient())
                    //{
                    //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    //    wc.Headers.Add("Authorization", ConfigurationManager.AppSettings["PaymentAuthorizationKey"].ToString());
                    //    string HtmlResult = wc.UploadString(URI, myParameters);
                    //    var jss = new JavaScriptSerializer();
                    //    var dict = jss.Deserialize<Dictionary<dynamic, dynamic>>(HtmlResult);
                    //    Console.WriteLine(dict["message"]);
                    //}

                    //string URI = ConfigurationManager.AppSettings["PaymentURL"].ToString() + "treasury/merchant/refundPayment?";
                    //string myParameters = "merchantKey=" + ConfigurationManager.AppSettings["PaymentKey"].ToString() + "&paymentId=" + paymentId + "&refundAmount=" + RefundAmount;


                    //using (WebClient wc = new WebClient())
                    //{
                    //    wc.Headers[HttpRequestHeader.ContentType] = "application/json"; // "application/x-www-form-urlencoded";
                    //    wc.Headers.Add("Authorization", ConfigurationManager.AppSettings["PaymentAuthorizationKey"].ToString());
                    //    string HtmlResult = wc.UploadString(URI, myParameters);
                    //    var jss = new JavaScriptSerializer();
                    //    var dict = jss.Deserialize<Dictionary<dynamic, dynamic>>(HtmlResult);
                    //    Console.WriteLine(dict["message"]);
                    //}


                    //using (WebClient client = new WebClient())
                    //{
                    //    // Request configuration     

                    //    client.Headers.Add("Authorization", ConfigurationManager.AppSettings["PaymentAuthorizationKey"].ToString());
                    //    client.Headers.Add("Accept", "application/json");
                    //    client.Headers.Add("Content-Type", "application/json");
                    //    string HtmlResult = client.UploadString(URI,"Post", myParameters);
                       
                    //}
                    if (_FrontFarmStaysBookingService.ChangeOrderStatus(Id, (int)BookingStatus.Cancel, RefundAmount, CancellationReason))
                    {
                        #region EmailTemplate
                        string CurrencyCode = Helper.GetCurrentCurrency();
                        FrontOrdersService _OrdersService = new FrontOrdersService();
                        FrontOrdersMaster ordersMaster = _OrdersService.GetOrderDetailByKey(Id, CurrencyCode);

                        EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.BookingCancellation);
                        if (_EmailTemplates != null)
                        {
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", ordersMaster.GuestFirstName);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#FarmStayName#", ordersMaster.FarmStaysName);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CancellationReason#", CancellationReason);

                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Name#", ordersMaster.Name);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Type#", ordersMaster.TypeName);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckIn#", ordersMaster.CheckInDate.ToString("dd/mm/yyyy"));
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckOut#", ordersMaster.CheckOutDate.ToString("dd/mm/yyyy"));
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Guests#", ordersMaster.NoOfPeople.ToString());
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Price#", "₹" + (ordersMaster.DiscountAmount > 0 ? ordersMaster.DiscountAmount : ordersMaster.Amount).ToString());
                          

                            bool resultMail = Helper.SendMail(ordersMaster.GuestEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                        }
                        EmailTemplates _AdminEmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.AdminBookingCancellation);
                        if (_AdminEmailTemplates != null)
                        {
                            string adminEmail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", ordersMaster.GuestFirstName);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#FarmStayName#", ordersMaster.FarmStaysName);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CancellationReason#", CancellationReason);

                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Name#", ordersMaster.Name);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Type#", ordersMaster.TypeName);
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckIn#", ordersMaster.CheckInDate.ToString("dd/mm/yyyy"));
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckOut#", ordersMaster.CheckOutDate.ToString("dd/mm/yyyy"));
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Guests#", ordersMaster.NoOfPeople.ToString());
                            _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Price#", "₹" + (ordersMaster.DiscountAmount > 0 ? ordersMaster.DiscountAmount : ordersMaster.Amount).ToString());


                            bool resultMail = Helper.SendMail(adminEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                        }
                        #endregion

                        //No need to delete entry - Labdhi
                        //InventoryMasterService _InventoryMasterService = new InventoryMasterService();
                        //_InventoryMasterService.DeleteInventoryByOrderId(Id);
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHashKey(HashKeyResponse data)
        {
            byte[] hash;
            //string postData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            //dynamic data = JsonConvert.DeserializeObject(postData);
            string d = data.key + "|" + data.txnid + "|" + data.amount + "|" + data.pinfo + "|" + data.fname + "|" + data.email + "|||||" + data.udf5 + "||||||" + data.salt;
            var datab = Encoding.UTF8.GetBytes(d);
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(datab);
            }

            return Json(GetStringFromHash(hash), JsonRequestBehavior.AllowGet);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2").ToLower());
            }
            return result.ToString();
        }
        [HttpPost]
        public ActionResult OrderPayment(BookingPaymentResponse BookingPaymentResponse)
        {
            if (BookingPaymentResponse != null && BookingPaymentResponse.OrderID>0)
            {
                int _Status;
                if( BookingPaymentResponse.Status=="success")
                    _Status=(int)BookingStatus.CONFIRMED;
                else
                    _Status=(int)BookingStatus.Failed;

                bool _PaymentResult = _FrontFarmStaysBookingService.AddPaymentHistory(BookingPaymentResponse.OrderID, _Status,BookingPaymentResponse.PayuMoneyId, BookingPaymentResponse.PaymentResponse);

                bool _Result = _FrontFarmStaysBookingService.ChangeOrderStatus(BookingPaymentResponse.OrderID, _Status);
                if (_Result && BookingPaymentResponse.OrderID > 0 && _Status == (int)BookingStatus.CONFIRMED)
                {
                        string _Url = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"Orders/ViewGuestOrderDetail?OId=" + Helper.Encrypt(BookingPaymentResponse.OrderID.ToString());
                      
                    string CurrencyCode = Helper.GetCurrentCurrency();
                    FrontOrdersService _OrdersService = new FrontOrdersService();
                    EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.BookingConfirmation);
                    if (_EmailTemplates != null)
                    {
                        DateTime _Refund = Convert.ToDateTime(BookingPaymentResponse.CheckIn).AddDays(-BookingPaymentResponse.RefundableBeforDays).AddMinutes(-1);
                        string _CancellationPolicy = (BookingPaymentResponse.CancellationPolicyIsNonRefundable || !BookingPaymentResponse.IsApplyCancellationPolicy) ? "Non Refundable" : (BookingPaymentResponse.RefundablePercentage + "% will refunded, if you cancel the booking before " + _Refund.ToString("dd MMMM, yyyy hh:mm tt"));
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", BookingPaymentResponse.GuestFirstName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#FarmStayName#", BookingPaymentResponse.FarmStayName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Name#", BookingPaymentResponse.Name);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Type#", BookingPaymentResponse.TypeName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckIn#", BookingPaymentResponse.CheckIn);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckOut#", BookingPaymentResponse.CheckOut);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Guests#", BookingPaymentResponse.Guests.ToString());
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Address#", BookingPaymentResponse.Address);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Price#", "₹" + (BookingPaymentResponse.DiscountPrice > 0 ? BookingPaymentResponse.DiscountPrice : BookingPaymentResponse.Price).ToString());
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CancellationPolicy#", _CancellationPolicy);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#OrderURL#", _Url);

                        bool resultMail = Helper.SendMail(BookingPaymentResponse.GuestEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                        return Json(Helper.Encrypt(BookingPaymentResponse.OrderID.ToString() + '&' + DateTime.Now), JsonRequestBehavior.AllowGet);
                    }
                    EmailTemplates _AdminEmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.AdminBookingConfirmation);
                    if (_AdminEmailTemplates != null)
                    {
                        string adminEmail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();
                        DateTime _Refund = Convert.ToDateTime(BookingPaymentResponse.CheckIn).AddDays(-BookingPaymentResponse.RefundableBeforDays).AddMinutes(-1);
                        string _CancellationPolicy = (BookingPaymentResponse.CancellationPolicyIsNonRefundable || !BookingPaymentResponse.IsApplyCancellationPolicy) ? "Non Refundable" : (BookingPaymentResponse.RefundablePercentage + "% will refunded, if you cancel the booking before " + _Refund.ToString("dd MMMM, yyyy hh:mm tt"));
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", BookingPaymentResponse.GuestFirstName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#FarmStayName#", BookingPaymentResponse.FarmStayName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Name#", BookingPaymentResponse.Name);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Type#", BookingPaymentResponse.TypeName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckIn#", BookingPaymentResponse.CheckIn);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CheckOut#", BookingPaymentResponse.CheckOut);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Guests#", BookingPaymentResponse.Guests.ToString());
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Address#", BookingPaymentResponse.Address);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#Price#", "₹" + (BookingPaymentResponse.DiscountPrice > 0 ? BookingPaymentResponse.DiscountPrice : BookingPaymentResponse.Price).ToString());
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#CancellationPolicy#", _CancellationPolicy);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#OrderURL#", _Url);

                        bool resultMail = Helper.SendMail(adminEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                        return Json(Helper.Encrypt(BookingPaymentResponse.OrderID.ToString() + '&' + DateTime.Now), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool AxisBookingAPIIntegration(AxisBookingResponse _AxisBookingList)
        {
            if (_AxisBookingList != null)
            {
                List<AxisBookingResponse> _NewList = new List<AxisBookingResponse>();
                _NewList.Add(_AxisBookingList);
                bool _IsIntegrated = _FrontFarmStaysBookingService.AxisBokingAPIIntegration(_NewList);
                return true;
            }
            return false;
        }
    }
}