namespace HSBM.EntityModel.InventoryMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;
    using HSBM.Common.Utils;
    using System.Globalization;

    public class InventoryMasterResponse : InventoryMaster
    {
        public int RecordsTotal { get; set; }
    }

    public class InventoryAvailable
    {
        public int RecordsTotal { get; set; }
        private string _BookDate;
        public string BookDate { get { return string.IsNullOrEmpty(_BookDate) ? "" : Convert.ToDateTime(_BookDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _BookDate = value; } }
        public long RoomId { get; set; }
        public string RoomName { get; set; }
        public int MaxPerson { get; set; }
        public int Booked { get; set; }
        public int Available { get { return MaxPerson - Booked; } }
        public decimal Price { get; set; }
    }

    public class UpCommingOrder
    {


        public int Id { get; set; }

        private string _CheckInDate;
      
        public string CheckInDate { get { return string.IsNullOrEmpty(_CheckInDate) ? "" : Convert.ToDateTime(_CheckInDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _CheckInDate = value; } }

        private string _CheckOutDate;

        public string CheckOutDate { get { return string.IsNullOrEmpty(_CheckOutDate) ? "" : Convert.ToDateTime(_CheckOutDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _CheckOutDate = value; } }

        public int NoOfPeople { get; set; }

        public string OrderNo { get; set; }

        public int RecordsTotal { get; set; }

    }
}