using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HSBM.Web.Models
{
    public class FarmStaysRequestModel
    {

        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime? CheckIn { get; set; }

        //[Required]
        //[DataType(DataType.Date)]
        //[DateValidation("End date must be greater than start date.", compareWith: "CheckIn")]
        //public DateTime? CheckOut { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

        public int Guests { get; set; }

        public int Child { get; set; }

        public bool IsSolo { get; set; }

        public long FarmStayId { get; set; }

        public bool IsPackage { get; set; }
    }
}