using BLToolkit.Mapping;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class FarmStaysRatingsAndReview
    {
        public long Id { get; set; }
        public long FarmStyasId { get; set; }
        public long Customerid { get; set; }
        public decimal Ratings { get; set; }
        public string Reviews { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderNo { get; set; }
        public Guid FarmStaysRatingsAndReviewGUID { get; set; }
        public int IsApproved { get; set; }
    }

    public class FarmStaysRatingsAndReviewRequest
    {
        public int FarmStyasId { get; set; }
    }

    public class FarmStaysRatingsAndReviewResponse
    {
        public long Id { get; set; }
        [DisplayName("Farm/Home Stays Name")]
        public string FarmStyasName { get; set; }
        [DisplayName("Review By")]
        public string ReviewBy { get; set; }
        [DisplayName("Review Date")]
        public DateTime ReviewDate { get; set; }
        [MapIgnore]
        [DisplayName("Review Date")]
        public string strReviewDate
        {
            get
            {
                try
                { return ReviewDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); }
                catch (Exception)
                { return ""; }
            }
        }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        [MapIgnore]
        [DisplayName("Approved Date")]
        public string strApprovedDate
        {
            get
            {
                try
                { return (ApprovedDate.HasValue ? (ApprovedDate.Value.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture)) : ""); }
                catch (Exception)
                { return ""; }
            }
        }
        public decimal Ratings { get; set; }
        public string Reviews { get; set; }
        public int IsApproved { get; set; }

        public int RecordsTotal { get; set; }
        public decimal Location { get; set; }
        public decimal Cleanliness { get; set; }
        public decimal ValueForMoney { get; set; }
        public decimal Hospitality { get; set; }
    }
}
