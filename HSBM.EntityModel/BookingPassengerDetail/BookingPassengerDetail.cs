namespace HSBM.EntityModel.BookingPassengerDetail
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using HSBM.Common.Enums;

    public class BookingPassengerDetail
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNumber { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Email { get; set; }
        public string PassengerType { get; set; } // GuestType
        public bool WithInfant { get; set; }
        public long RoomId { get; set; }
        public long RoomNo { get; set; }
        public string AdultChild {get; set;} // AgeBand
        public string Title { get; set; }
        public int Age { get; set; }
        public bool IsLeadTraveler { get; set; }
        public bool IsAdditionalDriver { get; set; }
        public string LicenceNumber { get; set; }

        public AgeBand AgeBand { get; set; }

    }

}