using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Notification
{
    public class NotificationDetail
    {
        public long Id { get; set; }

        public long NotificationMasterId { get; set; }

        public long SystemUserId { get; set; }

        public string IsViewed { get; set; }

        public string IsDeleted { get; set; }

        public string CreatedDate { get; set; }
    }
}
