using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.PaymentGetWay
{
    public class PaypalPaymentRequest
    {
        public string cmd { get; set; }

        public string FormUrl { get; set; }

        public string business { get; set; }

        public string currency_code { get; set; }

        public string return_url { get; set; }

        public string notify_url { get; set; }

        public string cancel_return { get; set; }

        public Guid ProductID { get; set; }

        public string item_number { get; set; }

        public Guid IPNId { get; set; }

        public int LicenseSeats { get; set; }

        public string item_name { get; set; }

        public string amount { get; set; }

        public decimal tax { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string zip { get; set; }

        public string state { get; set; }

        public string country { get; set; }

        public string email { get; set; }

        public string Company { get; set; }

        public int quantity { get; set; }

        public string StateAbr { get; set; }

        public string CountryAbr { get; set; }

        public bool SeatUpgrade { get; set; }

        public Guid OrderDetailKey { get; set; }

        public string IsSupplier { get; set; }

        public string Custom { get; set; }

        public string Mobile { get; set; }

        public string UserName { get; set; }
               
    }
}
