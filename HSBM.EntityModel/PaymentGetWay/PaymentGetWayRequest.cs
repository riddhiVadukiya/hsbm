using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.PaymentGetWay
{
    public class PaymentGetWayRequest
    {
        public int PaymentTypeId { get; set; }

        public CCAvenueRequest CCAvenueModel { get; set; }

        public PaypalPaymentRequest PaypalModel { get; set; }

        public SagePayRequest SagePayModel { get; set; }
    }
}
