using CCA.Util;
using HSBM.Common.Enums;
using HSBM.EntityModel.PaymentGetWay;
using HSBM.EntityModel.SystemUsers;
using HSBM.Service.Services;
using HSBM.Service.Services.SystemUserServices;
using HSBM.Web.Helpers;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class PaymentController : BaseController
    {
        PaymentGetWayRequest _PaymentGetWayRequest = new PaymentGetWayRequest();
        // GET: Payment
        public ActionResult Index()
        {
            SystemUserService _SystemUserService = new SystemUserService();
            PaypalPaymentRequest _PaypalPaymentRequest = new PaypalPaymentRequest();
            SystemUsers _SystemUsers = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, SessionProxy.UserDetails.Id);

            return View(_SystemUsers);
        }

        public ActionResult Paypal(PaypalPaymentRequest p_PaypalPaymentRequest)
        {
            //p_PaypalPaymentRequest.currency_code = SessionProxy.BaseCurrency;
            return View(p_PaypalPaymentRequest);
        }

        //[HttpPost]
        //public ActionResult PaymentGetway(string Amount, int paymentTypeId , string ReturnURL)
        //{
        //    Random random = new Random();
        //    _PaymentGetWayRequest.PaymentTypeId = paymentTypeId;

        //    if (paymentTypeId > 0 && paymentTypeId == (int)PaymentGateways.PayPal)
        //    {
        //        PaypalPaymentRequest _PaypalPaymentRequest = new PaypalPaymentRequest();
        //        _PaypalPaymentRequest.currency_code = System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalCurrency"];
        //        _PaypalPaymentRequest.FormUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["SandBoxUrl"];
        //        _PaypalPaymentRequest.return_url = ReturnURL;
        //        _PaypalPaymentRequest.business = System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalMerchant"];
        //        _PaypalPaymentRequest.notify_url = System.Web.Configuration.WebConfigurationManager.AppSettings["ReturnURL"];
        //        _PaypalPaymentRequest.amount = Amount;
        //        _PaypalPaymentRequest.item_name = "test123";
        //        _PaypalPaymentRequest.cmd = "_xclick";
        //        _PaymentGetWayRequest.PaypalModel = _PaypalPaymentRequest;

        //        return RedirectToAction("Paypal", _PaymentGetWayRequest.PaypalModel);
        //    }
        //    else if (paymentTypeId > 0 && paymentTypeId == (int)PaymentGateways.CCAvenue)
        //    {

        //        CCACrypto _CCACrypto = new CCACrypto();
        //        CCAvenueRequest _CCAvenue = new CCAvenueRequest();
        //        _CCAvenue.Currency = "INR";
        //        _CCAvenue.Language = "EN";
        //        _CCAvenue.Amount = Convert.ToDecimal(Amount);
        //        _CCAvenue.CCAvenueUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["CCAvenueUrl"];
        //        _CCAvenue.MerchantId = System.Web.Configuration.WebConfigurationManager.AppSettings["CCAvenueMerchantId"];
        //        _CCAvenue.AccessCode = System.Web.Configuration.WebConfigurationManager.AppSettings["CCAvenueAccessCode"];
        //        _CCAvenue.Workingkey = System.Web.Configuration.WebConfigurationManager.AppSettings["CCAvenueWorkingkey"];
        //        _CCAvenue.RedirectURL = new Uri(System.Web.HttpContext.Current.Request.Url, ConfigurationManager.AppSettings["CCAvenueReturnURL"]).ToString();
        //        _CCAvenue.CancelURL = new Uri(System.Web.HttpContext.Current.Request.Url, ConfigurationManager.AppSettings["CCAvenueCancelURL"]).ToString();
        //        _CCAvenue.TId = random.Next();
        //        _CCAvenue.OrderId = Guid.NewGuid().ToString();
        //        string reqStr = "tid=" + _CCAvenue.TId + "&merchant_id=" + _CCAvenue.MerchantId + "&order_id=" + _CCAvenue.OrderId + "&amount=" + _CCAvenue.Amount + "&currency=" + _CCAvenue.Currency + "&redirect_url=" + _CCAvenue.RedirectURL + " &cancel_url=" + _CCAvenue.CancelURL + "&language=" + _CCAvenue.Language + "";
        //        _CCAvenue.EncRequest = _CCACrypto.Encrypt(reqStr, _CCAvenue.Workingkey);


        //        return RedirectToAction("CCAvenuePayment", _CCAvenue);
        //    }
        //    else if (paymentTypeId > 0 && paymentTypeId == (int)PaymentGateways.SagePay)
        //    {
        //        SagePayRequest _SagePayModel = new SagePayRequest();
        //        _SagePayModel.VendorTxCode = Guid.NewGuid().ToString();
        //        _SagePayModel.Amount = Convert.ToDecimal(Amount);
        //        _SagePayModel.Currency = "GBP";                
        //        _SagePayModel.NotificationURL = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "Payment/SagePayPaymentNotification";
        //        var serializer = new HttpPostSerializer();
        //        var postData = serializer.Serialize(_SagePayModel);

        //        //For Live: https://live.sagepay.com/gateway/service/vspserver-register.vsp
        //        string transactionRegistrationResponseStr = _SagePayModel.SendRequest("https://test.sagepay.com/gateway/service/vspserver-register.vsp", postData);

        //        var deserializer = new ResponseSerializer();
        //        TransactionRegistrationResponse transactionRegistrationResponse = deserializer.Deserialize<TransactionRegistrationResponse>(transactionRegistrationResponseStr);
        //        if (transactionRegistrationResponse.Status == SagepatResponseType.Ok)
        //        {
        //            //For Test Visa Card URL : https://www.sagepay.co.uk/support/12/36/test-card-details-for-your-test-transactions
        //            return Redirect(transactionRegistrationResponse.NextURL);
        //        }
        //    }
        //    else if (paymentTypeId > 0 && paymentTypeId == (int)PaymentGateways.AuthorizeNet)
        //    {
        //        AuthorizeNetRequest _AuthorizeNetRequest = new AuthorizeNetRequest();
        //        _AuthorizeNetRequest.Amount = Convert.ToDecimal(Amount);
        //        _AuthorizeNetRequest.Seq = _AuthorizeNetRequest.GenerateSequence();
        //        _AuthorizeNetRequest.Stamp = _AuthorizeNetRequest.GenerateTimestamp();
        //        _AuthorizeNetRequest.FingerPrint = _AuthorizeNetRequest.GenerateFingerprint(_AuthorizeNetRequest.TransactionKey, _AuthorizeNetRequest.ApiLogin, _AuthorizeNetRequest.Amount, _AuthorizeNetRequest.Seq, Convert.ToString(_AuthorizeNetRequest.Stamp));
        //        _AuthorizeNetRequest.OrderID = Guid.NewGuid().ToString();
        //        _AuthorizeNetRequest.TId = random.Next();

        //        return RedirectToAction("AuthorizeNet", _AuthorizeNetRequest);
        //    }
        //    return View("Index");
        //}

        public ActionResult SuccessPage()
        {
            return View();
        }

        public ActionResult SagePayPaymentNotification()
        {
            return View();
        }

        public ActionResult CCAvenuePayment(CCAvenueRequest p_CCAvenueRequest)
        {
            return View(p_CCAvenueRequest);
        }


        public ActionResult CancelTransaction()
        {
            return View();
        }

        public ActionResult AuthorizeNet(AuthorizeNetRequest p_AuthorizeNetRequest)
        {
            return View(p_AuthorizeNetRequest);
        }

        #region Paypal Payment Method

        public string GetAPIContext(string accessToken = "")
        {
            string token = new OAuthTokenCredential(System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalClientID"], System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalClientSecret"], GetConfig()).GetAccessToken();
            return token;
        }

        public ActionResult PaymentGetWay(string p_Amount, string ReturnURL, int p_OrderId, string p_Id) //, string p_Amount, string p_OrderId)
        {
            string accessToken = "";
            string payerId = Request.Params["PayerID"];

            try
            {
                accessToken = new OAuthTokenCredential(System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalClientID"], System.Web.Configuration.WebConfigurationManager.AppSettings["PaypalClientSecret"], GetConfig()).GetAccessToken();
                
                var apiContext = new APIContext(accessToken);
                apiContext.Config = GetConfig();

                if (string.IsNullOrEmpty(payerId))
                {
                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = CreatePayment(apiContext, ReturnURL, Convert.ToDecimal(p_Amount), p_Id, p_OrderId);
                    string _paypal = string.Empty;
                    var links = createdPayment.links.GetEnumerator();
                    while (links.MoveNext())
                    {
                        var link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            _paypal = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(_paypal);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var stateObj = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (stateObj.state.ToLower() != "approved")
                    {
                        return View();
                    }
                    else
                    {
                        if (stateObj.transactions[0] != null)
                        {
                            //AddTransaction(stateObj.id, Guid.Parse(stateObj.transactions[0].invoice_number), stateObj.transactions[0].related_resources[0].sale.id);
                            return RedirectToAction("ThankYou", "Payment", new { @p_OrderId = stateObj.transactions[0].description });
                        }
                        else
                        {
                            //AddTransaction(stateObj.id, Guid.Parse(stateObj.transactions[0].invoice_number), stateObj.transactions[0].related_resources[0].sale.id);
                            return RedirectToAction("CancelPayment", "Payment", new { @token = stateObj.id });
                        }
                    }
                }
            }
            catch (PayPal.IdentityException ex)
            {
                //AddTransaction _AddTransaction = new AddTransaction();
                //SaaS.DataTransfer.TransactionProto.Transaction _Transaction = new SaaS.DataTransfer.TransactionProto.Transaction();
                //_Transaction.OrderMasterId = Guid.Parse(Session["OrderMasterId"].ToString());
                //_Transaction.ResultCode = ((PayPal.HttpException)(ex)).StatusCode.ToString();
                //_Transaction.PaymentResponse = ((PayPal.IdentityException)(ex)).Details.error_description;
                //GenericCommonResponse _Response = _AddTransaction.Execute(WebRequestResponseServiceContext, _Transaction);
                //Session.Remove("OrderMasterId");
                _ILogger.Error(ex);
                return RedirectToAction("ErrorPage", "Payment");
            }
            catch (PayPal.PayPalException ex)
            {
                //AddTransaction _AddTransaction = new AddTransaction();
                //SaaS.DataTransfer.TransactionProto.Transaction _Transaction = new SaaS.DataTransfer.TransactionProto.Transaction();
                //_Transaction.OrderMasterId = Guid.Parse(Session["OrderMasterId"].ToString());
                //_Transaction.ResultCode = ((PayPal.PaymentsException)(ex)).Details.name;
                //_Transaction.PaymentResponse = ((PayPal.PaymentsException)(ex)).Details.message;
                //GenericCommonResponse _Response = _AddTransaction.Execute(WebRequestResponseServiceContext, _Transaction);
                //Session.Remove("OrderMasterId");
                _ILogger.Error(ex);
                return RedirectToAction("ErrorPage", "Payment");
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private Payment CreatePayment(APIContext p_APIContext, string p_redirectUrl, decimal p_Amount, string p_Id, int p_OrderId)
        {
            var itemList = new ItemList() { items = new List<Item>() };
            var transactionList = new List<Transaction>();
            var payer = new Payer() { payment_method = "paypal" };
            var redirectUrl = p_redirectUrl;
            itemList.items.Add(new Item()
            {
                name = "Item Name",
                currency = SessionProxy.BaseCurrency,
                price = Math.Round(p_Amount, 2).ToString(),
                quantity = "1",
                sku = "sku"
            });

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = Math.Round(p_Amount, 2).ToString()
            };

            var amount = new Amount()
            {
                currency = SessionProxy.BaseCurrency,
                total = Math.Round(p_Amount, 2).ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            transactionList.Add(new Transaction()
            {
                description = p_OrderId.ToString(),
                invoice_number = p_Id, // OrderMasterKey
                reference_id = p_OrderId.ToString(), // Custome Id of Order
                amount = amount,
                item_list = itemList
            });

            var redirUrls = new RedirectUrls()
            {
                cancel_url = "http://localhost:55734/Payment/CancelPayment",
                return_url = redirectUrl
            };
            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(p_APIContext);
        }

        public Payment ExecutePayment(APIContext p_APIContext, string p_payerId, string p_paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = p_payerId };

            var payment = new Payment() { id = p_paymentId };
            return payment.Execute(p_APIContext, paymentExecution);
        }

        //public void AddTransaction(string p_token, Guid p_OrderMasterId, string p_TransactionId)
        //{
        //    AddTransaction _AddTransaction = new AddTransaction();
        //    SaaS.DataTransfer.TransactionProto.Transaction _Transaction = new SaaS.DataTransfer.TransactionProto.Transaction();
        //    _Transaction.PaymentId = p_token;
        //    _Transaction.PaymentGatewayTransactionId = p_TransactionId;
        //    _Transaction.OrderMasterId = p_OrderMasterId;
        //    GenericCommonResponse _Response = _AddTransaction.Execute(WebRequestResponseServiceContext, _Transaction);
        //}

        //public ActionResult CancelPayment(string token)
        //{
        //    AddTransaction _AddTransaction = new AddTransaction();
        //    SaaS.DataTransfer.TransactionProto.Transaction _Transaction = new SaaS.DataTransfer.TransactionProto.Transaction();
        //    _Transaction.ResultCode = SaaS.Core.Util.ApplicationConstants.Cancelled;
        //    _Transaction.OrderMasterId = Guid.Parse(Session["OrderMasterId"].ToString());
        //    _Transaction.SystemUserId = SessionProxy.BuyerID;
        //    GenericCommonResponse _Response = _AddTransaction.Execute(WebRequestResponseServiceContext, _Transaction);
        //    Session.Remove("OrderMasterId");
        //    return View("Partial/_CancelPayment");
        //}

        public ActionResult ErrorPage()
        {
            return View("Partial/_ErrorPage");
        }

        #endregion


    }
}