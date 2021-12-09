using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class HotelController : BaseController
    {
        //private static readonly ISessionPool SessionPool = SessionPoolFactory.CreateSimple();

        //[Route("soap_workflow/search")]
        //public async Task<ActionResult> SoapWorkFlow(OTA_HotelAvailRQ requestModel)
        //{
        //    #region REQUEST
        //    List<OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef> list =
        //        new List<OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef>();
        //    list.Add(new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef()
        //                    {
        //                        ChainCode = "MC",
        //                        HotelCityCode = "FSG",
        //                        HotelCode = "1234567",
        //                        HotelName = "HOLIDAY"
        //                    });
        //    List<OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionRefPoint> list2 =
        //        new List<OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionRefPoint>();
        //    list2.Add(new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionRefPoint()
        //    {
        //        DistanceDirection = "9N",
        //        GeoCode = true,
        //        HotelCode = "1234567",
        //        Index = OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionRefPointIndex.C,
        //        Sort = true
        //    });
        //    List<OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionContactNumbers> list3 = new List<OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionContactNumbers>();
        //    list3.Add(new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionContactNumbers()
        //                    {
        //                        ContactNumber = new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionContactNumbersContactNumber() { Phone = "817-555-1212" }
        //                    });
        //    //<RefPoint DistanceDirection="9N" GeoCode="true" HotelCode="1234567" Index="C" Sort="true"/>
        //    requestModel = new OTA_HotelAvailRQ()
        //    {
        //        AvailRequestSegment = new OTA_HotelAvailRQAvailRequestSegment()
        //        {
        //            AdditionalAvail = new OTA_HotelAvailRQAvailRequestSegmentAdditionalAvail()
        //            {
        //                Ind = true
        //            },
        //            Customer = new OTA_HotelAvailRQAvailRequestSegmentCustomer()
        //            {
        //                Corporate = new OTA_HotelAvailRQAvailRequestSegmentCustomerCorporate() { ID = "ABC123" },
        //                ID = new OTA_HotelAvailRQAvailRequestSegmentCustomerID() { Number = "ABC123" }
        //            },
        //            GuestCounts = new OTA_HotelAvailRQAvailRequestSegmentGuestCounts()
        //            {
        //                Count = "2"
        //            },
        //            HotelSearchCriteria = new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteria()
        //            {
        //                NumProperties = "20",
        //                Criterion = new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterion()
        //                {
        //                    HotelRef = list.ToArray(),
        //                    Package = new string[] { "GF", "HM", "BB" },
        //                    PropertyType = new string[] { "APTS", "LUXRY" },
        //                    RefPoint = list2.ToArray(),
        //                    RoomAmenity = new string[] { "A2D", "**K" },
        //                    ContactNumbers = list3.ToArray(),
        //                },
        //            },
        //            POS = new OTA_HotelAvailRQAvailRequestSegmentPOS()
        //            {
        //                Source = new OTA_HotelAvailRQAvailRequestSegmentPOSSource()
        //                {
        //                    BookingChannel = new OTA_HotelAvailRQAvailRequestSegmentPOSSourceBookingChannel()
        //                    {
        //                        CompanyName = new OTA_HotelAvailRQAvailRequestSegmentPOSSourceBookingChannelCompanyName()
        //                        {
        //                            Division = "HGV"
        //                        }
        //                    }
        //                }
        //            },

        //        },


        //    };
        //    #endregion

        //    HotelService service = new HotelService();
        //    OTA_HotelAvailRQResponse rsp = service.HotelAvailabilityService();

        //    IActivity activity = new InitialSoapActivity(new SoapServiceFactory(ConfigFactory.CreateForRest()), SessionPool, new SoapWorkflowPostRQ() { });
        //    Workflow workflow = new Workflow(activity);
        //    SharedContext sharedContext = await workflow.RunAsync();
        //    SoapWorkflowVM model = ViewModelFactory.CreateSoapWorkflowVM(sharedContext);
        //    return this.View(model);
        //}

        //   return View();
        //}

        /// <summary>
        /// Search page of Hotel
        /// </summary>
        /// <param name="APIProvider"> Supplier Id </param>
        /// <returns> View </returns>
        [Route("Hotel/{APIProvider}")]
        public ActionResult Index(int? APIProvider)
        {
            return View();
        }

        
        
        

        
      
    }
}
