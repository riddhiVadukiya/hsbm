using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.PaymentGetWay
{
    public class CCAvenueRequest
    {
        public string CCAvenueUrl { get; set; }
        
        public string MerchantId { get; set; }
        
        public string AccessCode { get; set; }
        
        public string Workingkey { get; set; }
        
        public string RedirectURL { get; set; }

        public string CancelURL { get; set; }

        public string OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Language { get; set; }

        public int TId { get; set; }

        public string EncRequest { get; set; }


        //public string GetEncRequest()
        //{
        //    CCACrypto ccaCrypto = new CCACrypto();
        //    string reqStr = "tid=" + this.TId + "&merchant_id=" + this.MerchantId + "&order_id=" + this.OrderId + "&amount=" + this.Amount + "&currency=" + this.Currency + "&redirect_url=" + this.RedirectURL + " &cancel_url=" + this.CancelURL + "&language=" + this.Language + "";
        //    return ccaCrypto.Encrypt(reqStr, this.Workingkey);
        //}
    }
}
