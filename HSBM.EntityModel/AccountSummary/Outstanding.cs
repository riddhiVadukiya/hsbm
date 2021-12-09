namespace HSBM.EntityModel.AccountSummary
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;
    using System.Globalization;

    public class Outstanding
    {
        public long Farmstaysid { get; set; }

        public string FarmstaysName { get; set; }

        public int BookingYear { get; set; }

        public int BookingMonth { get; set; }

        public string BookingMonthandYear
        {
            get
            {
                return new DateTime(2010, BookingMonth, 1).ToString("MMMM", CultureInfo.InvariantCulture) + " " + BookingYear;
            }
        }

        public int TotalBooking { get; set; }

        public decimal TotalOutstanding { get; set; }

        public int RecordsTotal { get; set; }

        public bool IsCleared { get; set; }

        public long OutstandingId { get; set; }

        public string TotalOutstandingString { get; set; }

        public string Status { get; set; }
    }

    public class OutstandingRequest
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int FarmStyasId { get; set; }

        public string FarmStaysName { get; set; }
    }

    public class OutstandingHistoryRequest
    {
        public int startMonth { get; set; }
        public int startYear { get; set; }
        public int endMonth { get; set; }
        public int endYear { get; set; }
        public int FarmstyaId { get; set; }
        public string FarmStaysName { get; set; }

    }

}
