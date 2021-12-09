using BLToolkit.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HSBM.EntityModel.FrontReview
{
    public class AddReviewModel
    {
        public long FarmStyasId { get; set; }
        public string FarmStyasName { get; set; }
        public int Customerid { get; set; }
        public string OrderNo { get; set; }        
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid FarmStaysRatingsAndReviewGUID { get; set; }
        public bool IsInserted { get; set; }
        
        [StringLength(1000)]        
        [Display(Name = "Reviews")]         
        public string Reviews { get; set; }

        [Display(Name = "Rating")]
        public decimal Rating { get; set; }

        public bool IsFromEmail { get; set; }
        public decimal Location { get; set; }
        public decimal Cleanliness { get; set; }
        public decimal ValueForMoney { get; set; }
        public decimal Hospitality { get; set; }
    }
}
