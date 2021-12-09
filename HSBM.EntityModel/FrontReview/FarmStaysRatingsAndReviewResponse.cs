using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FrontReview
{
    public class FarmStaysRatingsAndReviewResponse
    {
        public long Id { get; set; }
        public long FarmStyasId { get; set; }
        public long Customerid { get; set; }
        public decimal Ratings { get; set; }
        public string Reviews { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderNo { get; set; }
        public Guid FarmStaysRatingsAndReviewGUID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Location { get; set; }
        public decimal Cleanliness { get; set; }
        public decimal ValueForMoney { get; set; }
        public decimal Hospitality { get; set; }
    }
}


