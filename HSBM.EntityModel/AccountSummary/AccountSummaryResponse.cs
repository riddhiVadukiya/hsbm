namespace HSBM.EntityModel.AccountSummary
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class AccountSummaryResponse
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public string TransactionNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public decimal MarkupAmount { get; set; }

        public int SupplierType { get; set; }

        public string CompanyName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string OrderType { get; set; }

    }

    public class OutstandingSummary
    {
        public string CompanyName { get; set; }

        public int TotalBookings { get; set; }

        public string Currency { get; set; }

        public decimal Outstanding { get; set; }

    }

}
