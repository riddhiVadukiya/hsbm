namespace HSBM.EntityModel.InventoryMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class InventoryMasterRequest
    {
        public long Id { get; set; }

        public long FarmstaysId { get; set; }

        public string BookingDate { get; set; }

        public int NumberOfProperty { get; set; }

        public bool OnSite { get; set; }

        public long OrderId { get; set; }

        public long RoomId { get; set; }

        public Guid BookingGroupId { get; set; }

    }


}