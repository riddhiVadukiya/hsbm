namespace HSBM.EntityModel.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HSBM.EntityModel.SubscriptionMaster;

    public class Newsletter
    {

        public int EmailTemplateId { get; set; }

        public List<SubscriptionMaster> Users { get; set; }

        public string TemplateHtml { get; set; }

        public string Subject { get; set; }

    }
}
