using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FrontOrdersMasterRequest
    {
        public long Id { get; set; }

        public string OrderNo { get; set; }

        public DateTime OrderDate { get; set; }

        public string FarmStaysName { get; set; }

        public decimal NetAmount { get; set; }

        public bool IsActive { get; set; }

        public long Farmstaysid { get; set; }

        public string CreatedDateFrom { get; set; }

        public string CreatedDateTo { get; set; }

        public string CustomerName { get; set; }

        public long CustomerId { get; set; }

        public long OrderStatusId { get; set; }

        public string CurrencyCode { get; set; }
    }
}
