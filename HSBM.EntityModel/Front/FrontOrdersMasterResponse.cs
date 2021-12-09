using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FrontOrdersMasterResponse : FrontOrdersMaster
    {
        public int RecordsTotal { get; set; }

        public Guid FarmStaysRatingsAndReviewGUID { get; set; }

        public string OrderNoEncrypt { get { return Helper.Encrypt(OrderNo); } }

        public string OrderIdEncrypt { get { return Helper.Encrypt(Id.ToString()); } }
    }
}
