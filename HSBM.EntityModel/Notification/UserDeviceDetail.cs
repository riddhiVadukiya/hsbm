using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Notification
{
    public class UserDeviceDetail
    {
        public long Id { get; set; }

        public long SystemUserId { get; set; }

        public string DeviceId { get; set; }

        public string GCMRegistrationId { get; set; }

        public bool IsNotificationAllow { get; set; }

        public string Provider { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
