namespace HSBM.EntityModel.AccountSummary
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;
    using System.Globalization;
    using HSBM.Common.Utils;

    public class AccountStatement
    {
        public long Farmstaysid { get; set; }

        public string FarmstaysName { get; set; }

        public string BookingDate { get; set; }

        public string CheckInDate { get; set; }

        //public int BookingYear { get; set; }

        //public int BookingMonth { get; set; }

        //public string BookingMonthandYear
        //{
        //    get
        //    {
        //        return new DateTime(2010, BookingMonth, 1).ToString("MMMM", CultureInfo.InvariantCulture) + " " + BookingYear;
        //    }
        //}

        //public int CheckInYear { get; set; }

        //public int CheckInMonth { get; set; }

        //public string CheckInMonthandYear
        //{
        //    get
        //    {
        //        return new DateTime(2010, CheckInMonth, 1).ToString("MMMM", CultureInfo.InvariantCulture) + " " + CheckInYear;
        //    }
        //}

        public int TotalBooking { get; set; }

        public decimal TotalEarning { get; set; }

        public int RecordsTotal { get; set; }

        public string TotalEarningString { get; set; }
    }

    public class AccountStatementRequest
    {
        public string BookingFrom { get; set; }

        public string BookingTo { get; set; }

        public int FarmStyasId { get; set; }

        public string CheckInFrom { get; set; }

        public string CheckInTo { get; set; }

        public string FarmStaysName { get; set; }


    }

    public class AccountStatementHistoryRequest
    {
        public int startMonth { get; set; }
        public int startYear { get; set; }
        public int endMonth { get; set; }
        public int endYear { get; set; }
        public int FarmstyaId { get; set; }
        public int checkinFromMonth { get; set; }
        public int checkinFromYear { get; set; }
        public int checkinToMonth { get; set; }
        public int checkinToYear { get; set; }

    }

}
