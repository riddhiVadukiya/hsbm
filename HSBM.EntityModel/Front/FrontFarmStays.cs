using HSBM.EntityModel.FarmStays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FrontFarmStays
    {
        public long Id { get; set; } //(bigint, not null)
        public string Name { get; set; } //(nvarchar(50), not null)
        public int CategoryId { get; set; } //(int, not null)
        public string Email { get; set; } //(nvarchar(max), not null)
        public string Description { get; set; } //(nvarchar(max), not null)
        public int Bedrooms { get; set; } //(int, not null)
        public int MaxGuests { get; set; } //(int, not null)
        public int Beds { get; set; } //(int, not null)
        public int Bathrooms { get; set; } //(int, not null)
        public string CheckInTime { get; set; } //(time(7), not null)
        public string CheckOutTime { get; set; } //(time(7), not null)
        public string Telephone { get; set; } //(nvarchar(50), null)
        public string Mobile { get; set; } //(nvarchar(50), null)
        public int NumberOfProperty { get; set; } //(int, not null)
        public decimal DefultPrice { get; set; } //(money, not null)
        public decimal? ExtraBedPrice { get; set; } //(money, null)
        public decimal CleaningFee { get; set; } //(money, null)
        public decimal ConvenienceFee { get; set; } //(money, null)
        public string Address { get; set; } //(nvarchar(250), not null)
        public string HouseRules { get; set; } //(nvarchar(max), null)
        public string StayPolicy { get; set; } //(nvarchar(max), null)
        public bool CancellationPolicyIsNonRefundable { get; set; } //(bit, not null)
        public decimal RefundablePercentage { get; set; } //(decimal(3,2), null)
        public int RefundableBeforDays { get; set; } //(int, null)
        public decimal MarkupPercentage { get; set; } //(decimal(3,2), null)
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; } //(bit, not null)
        public long CreatedBy { get; set; } //(bigint, not null)
        public DateTime CreatedDate { get; set; } //(datetime, not null)
        public long UpdatedBy { get; set; } //(bigint, null)
        public DateTime UpdatedDate { get; set; } //(datetime, null)

        public List<FarmStaysAmenities> FarmStaysAmenities { get; set; }
        public List<FarmStaysImages> FarmStaysImages { get; set; }
        //public List<InventoryMaster> InventoryMaster { get; set; }
        public List<FarmStaysSeasons> FarmStaysSeasons { get; set; }
    }
    public class FarmStaysBasicDetail
    {
        public long Id { get; set; } //(bigint, not null)
        public string Name { get; set; } //(nvarchar(50), not null)
        public int CategoryId { get; set; } //(int, not null)
        public string Email { get; set; } //(nvarchar(max), not null)
        public string Description { get; set; } //(nvarchar(max), not null)
        public int Bedrooms { get; set; } //(int, not null)
        public string CheckInTime { get; set; } //(time(7), not null)
        public string CheckOutTime { get; set; } //(time(7), not null)
        public string Telephone { get; set; } //(nvarchar(50), null)
        public string Mobile { get; set; } //(nvarchar(50), null)
        public decimal? ExtraBedPrice { get; set; } //(money, null)
        public string Address { get; set; } //(nvarchar(250), not null)
        public decimal MarkupPercentage { get; set; } //(decimal(3,2), null)
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; } //(bit, not null)
        public long CreatedBy { get; set; } //(bigint, not null)
        public DateTime CreatedDate { get; set; } //(datetime, not null)
        public long UpdatedBy { get; set; } //(bigint, null)
        public DateTime UpdatedDate { get; set; } //(datetime, null)

    }

    public class FarmStaysPolicyDetail
    {
        public long Id { get; set; }
        public string HouseRules { get; set; } //(nvarchar(max), null)
        public string StayPolicy { get; set; } //(nvarchar(max), null)
        public bool CancellationPolicyIsNonRefundable { get; set; } //(bit, not null)
        public decimal RefundablePercentage { get; set; } //(decimal(3,2), null)
        public int RefundableBeforDays { get; set; } //(int, null)
        public long UpdatedBy { get; set; } //(bigint, null)
        public DateTime UpdatedDate { get; set; } //(datetime, null)
    }
}
