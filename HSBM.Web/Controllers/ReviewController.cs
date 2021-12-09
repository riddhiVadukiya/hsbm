using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.EmailTemplates;
using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontReview;
using HSBM.EntityModel.OrdersMaster;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class ReviewController : BaseController
    {
        FrontFarmStaysReviewService _FrontFarmStaysReviewService = new FrontFarmStaysReviewService();
        //
        // GET: /Review/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddReview(string Token, string OrderId, bool IsFromEmail=false)
        {
            AddReviewModel addReviewModel = new AddReviewModel();
            try
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    string _OrderNo = Helper.Decrypt(Token);
                    FarmStaysRatingsAndReviewResponse obj = new FarmStaysRatingsAndReviewResponse();

                    obj = _FrontFarmStaysReviewService.GetReviewDetailByOrderNo(_OrderNo);

                    if (obj !=null && obj.Ratings != null && obj.Ratings > 0 && !string.IsNullOrEmpty(obj.Reviews))
                    {
                        string FarmStayName = _FrontFarmStaysReviewService.GetFarmStayNameById(obj.FarmStyasId);
                        addReviewModel.IsFromEmail = IsFromEmail;
                        addReviewModel.FarmStyasName = string.Empty;
                        addReviewModel.Rating = obj.Ratings;
                        addReviewModel.Reviews = obj.Reviews;
                        addReviewModel.FarmStaysRatingsAndReviewGUID = obj.FarmStaysRatingsAndReviewGUID;
                        addReviewModel.FarmStyasName = FarmStayName;
                        addReviewModel.FarmStyasId = obj.FarmStyasId;
                        addReviewModel.Location = obj.Location;
                        addReviewModel.Cleanliness = obj.Cleanliness;
                        addReviewModel.ValueForMoney = obj.ValueForMoney;
                        addReviewModel.Hospitality = obj.Hospitality;
                        addReviewModel.OrderNo = OrderId;
                        ViewBag.IsView = true;
                        return View(addReviewModel);
                    }
                    else
                    {

                        ViewBag.IsView = false;
                        addReviewModel.OrderNo = OrderId;
                        addReviewModel.IsFromEmail = IsFromEmail;
                        // addReviewModel.FarmStyasName = string.Empty;
                        //  addReviewModel.Rating = obj.Ratings;
                        // addReviewModel.Reviews = obj.Reviews;
                        // addReviewModel.FarmStaysRatingsAndReviewGUID = obj.FarmStaysRatingsAndReviewGUID;
                        //  addReviewModel.FarmStyasId = obj.FarmStyasId;
                        return View(addReviewModel);
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return View(addReviewModel);
        }

        public JsonResult AddReviewPopup(string OrderNo,int OrderId)
        {
            try
            {
                string _OrderId = Helper.Encrypt(OrderId.ToString());
                string _OrderNo = Helper.Encrypt(OrderNo.ToString());
                string _FormUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"Review/JsonAddReviewPopup";
                string _ReviewFormUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"/Review/AddReview?Token=" + _OrderNo + "&OrderId=" + _OrderId + "&IsFromEmail=true ";

                #region HTML Body
//                string _Html = @" <html>
//            <body>
//               <form action='#FormUrl#' method='post' target='_blank'>   
//                     <input type='hidden' name='OrderNo' value='#OrderNo#' />
//                    <label>Location: </label><br />
//                    <input name='Location' type='radio' value='1' />
//                    ★☆☆☆
//                    <br />
//                    
//                    <input name='Location' type='radio' value='2' />
//                    ★★☆☆
//                     <br />
//                   
//                    <input name='Location' type='radio' value='3'  />
//                   ★★★☆
//                    <br />
//                       
//                    <input name='Location' type='radio' value='4' />
//                    ★★★★                   
//                    <br />
// 
//                    <input name='Location' type='radio' value='5' checked />
//                    ★★★★★                    
//                    <br />
//                    <br />
//                     <label>Cleanliness: </label><br />
//                    <input name='Cleanliness' type='radio' value='1' />
//                    ★☆☆☆
//                    <br />
// 
//                    <input name='Cleanliness' type='radio' value='2' />
//                    ★★☆☆
//                     <br />
// 
//                    <input name='Cleanliness' type='radio' value='3' />
//                    ★★★☆
//                       <br />
// 
//                    <input name='Cleanliness' type='radio' value='4' />
//                    ★★★★                    
//                    <br />
// 
//                    <input name='Cleanliness' type='radio' value='5' checked  />
//                    ★★★★★   
//                    <br />
//                    <br />
//                     <label>ValueForMoney: </label><br />
//                    <input name='ValueForMoney' type='radio' value='1' />
//                    ★☆☆☆
//                    <br />
// 
//                    <input name='ValueForMoney' type='radio' value='2' />
//                    ★★☆☆
//                    <br />
// 
//                    <input name='ValueForMoney' type='radio' value='3' />
//                    ★★★☆
//                       <br />
// 
//                    <input name='ValueForMoney' type='radio' value='4' />
//                    ★★★★                    
//                    <br />
// 
//                    <input name='ValueForMoney' type='radio' value='5' checked  />
//                    ★★★★★ 
//                     <br />
//                    <br />
//                     <label>Hospitality: </label><br />
//                    <input name='Hospitality' type='radio' value='1' />
//                    ★☆☆☆
//                    <br />
// 
//                    <input name='Hospitality' type='radio' value='2' />
//                    ★★☆☆
//                    <br />
// 
//                    <input name='Hospitality' type='radio' value='3' />
//                    ★★★☆
//                       <br />
// 
//                    <input name='Hospitality' type='radio' value='4' />
//                    ★★★★                    
//                    <br />
// 
//                    <input name='Hospitality' type='radio' value='5' checked  />
//                    ★★★★★ 
//                    <br />
//                    <br />
//                    <label for='commentText'>Leave a quick review:</label><br />
//                    <textarea cols='75' id='description' name='description' rows='5'></textarea><br />
//                    <br />
//                    <input type='submit' value='Submit your review' />&nbsp;
//<br/>
//<br/>
//For write review in site <a href='#ReviewFormUrl#'>Click Here</a>.
//                </form>
//            </body>
//        </html>";
                #endregion

                EmailTemplateService emailTemplateService = new EmailTemplateService();
                EmailTemplates template = emailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.ReviewAndRatings);

                if (template != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    template.TemplatesHtml = template.TemplatesHtml.Replace("#OrderNo#", _OrderId);
                    template.TemplatesHtml = template.TemplatesHtml.Replace("#FormUrl#", _FormUrl);
                    template.TemplatesHtml = template.TemplatesHtml.Replace("#ReviewFormUrl#", _ReviewFormUrl);

                    bool resultMail = Helper.SendMail("riddhi.vadukiya@beelinesoftwares.com", "Test", template.TemplatesHtml, string.Empty, string.Empty);
                }

            }
            catch (Exception exception)
            {
            }
            return JsonSuccessResponse(false, "");
        }

        //[HttpPost]
        public ActionResult JsonAddReviewPopup(string OrderNo, string description, string rating, string Location, string Cleanliness, string ValueForMoney, string Hospitality, bool IsFromEmail=true)
        {
            try
            {
                if (!string.IsNullOrEmpty(OrderNo) && !string.IsNullOrEmpty(description) && Location != null && Cleanliness != null && ValueForMoney != null && Hospitality != null)
                {
                    Int32 _OrderNo = Convert.ToInt32( Helper.Decrypt(OrderNo));
                    OrdersService _OrdersService = new OrdersService();
                    OrdersMaster _OrdersMaster = _OrdersService.GetOrderDetailByKey(_OrderNo);

                    if (_OrdersMaster != null)
                    {
                        FarmStaysRatingsAndReviewResponse _FarmStaysRating = new FarmStaysRatingsAndReviewResponse();
                        _FarmStaysRating = _FrontFarmStaysReviewService.GetReviewDetailByOrderNo(_OrdersMaster.OrderNo);


                        //|| _FarmStaysRating.IsApproved
                        if (_FarmStaysRating == null)
                        {
                            FarmStaysRatingsAndReviewRequest obj = new FarmStaysRatingsAndReviewRequest();
                            obj.FarmStyasId = _OrdersMaster.Farmstaysid;
                            obj.Customerid = _OrdersMaster.CustomerId;
                            obj.OrderNo = _OrdersMaster.OrderNo;
                            obj.Reviews = description;
                            obj.Ratings = (Convert.ToDecimal(Location) + Convert.ToDecimal(Cleanliness) + Convert.ToDecimal(ValueForMoney) + Convert.ToDecimal(Hospitality)) / 4;
                            obj.FarmStaysRatingsAndReviewGUID = Guid.NewGuid();
                            // obj.IsApproved = 0;
                            obj.Location = Convert.ToDecimal(Location);
                            obj.Cleanliness = Convert.ToDecimal(Cleanliness);
                            obj.ValueForMoney = Convert.ToDecimal(ValueForMoney);
                            obj.Hospitality = Convert.ToDecimal(Hospitality);

                            int count = _FrontFarmStaysReviewService.AddRateAndReviews(obj);
                            if (count > 0)
                            {

                                TempData["MainMessge"] = "Thank You";
                                TempData["Message"] = WebRequestResponseServiceContext.Response.StatusMessage;
                               // return JsonSuccessResponse(true, WebRequestResponseServiceContext.Response.StatusMessage);
                            }
                            else
                            {
                                TempData["MainMessge"] = "Opss!!";
                                TempData["Message"] = WebRequestResponseServiceContext.Response.StatusMessage;
                               // return JsonSuccessResponse(false, WebRequestResponseServiceContext.Response.StatusMessage);
                            }
                        }
                        else
                        {
                            TempData["MainMessge"] = "Opss!!";
                            TempData["Message"] = "Review Already Exist";
                           // return JsonSuccessResponse(true, "Review Already Exist");
                        }
                    }
                    else
                    {
                        TempData["MainMessge"] = "Opss!!";
                        TempData["Message"] = "Data Not Found";
                       // return JsonSuccessResponse(true, "Data Not Found");
                    }
                }
                else
                {
                    TempData["MainMessge"] = "Opss!!";
                    TempData["Message"] = "Data Not Found";
                    // return JsonSuccessResponse(true, "Data Not Found");
                }
            }
            catch (Exception exception)
            {
            }
            if (IsFromEmail)
                return RedirectToAction("ReviewThankYou");
            else
                return JsonSuccessResponse(true, TempData["Message"].ToString());
        }

        public ActionResult ReviewThankYou()
        {
            FrontOrdersMaster ordersMaster = new FrontOrdersMaster();
            try
            {
               ViewBag.MainMessge= TempData["MainMessge"];
               ViewBag.Message = TempData["Message"];
               return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}