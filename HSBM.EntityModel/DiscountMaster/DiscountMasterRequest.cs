namespace HSBM.EntityModel.DiscountMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class DiscountMasterRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsPercentage { get; set; }

        public bool IsPreviousDiscounts { get; set; }

        public bool IsEBO { get; set; }

        //public DateTime? BookbyDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal DiscountValue { get; set; }

        public int DaysBeforeBooking { get; set; }

    }

}