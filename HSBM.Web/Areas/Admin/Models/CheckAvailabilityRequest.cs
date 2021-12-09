using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HSBM.Web.Areas.Admin.Models
{
    public class CheckAvailabilityRequest
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateValidation("End date must be greater than start date.", compareWith: "StartDate")]
        public DateTime? EndDate { get; set; }

        [Required]
        public long FarmStaysId { get; set; }

        public string FarmStaysName { get; set; }
    }
}