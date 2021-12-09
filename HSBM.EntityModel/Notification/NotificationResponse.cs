using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Notification
{
    public class NotificationResponse : Notification
    {
        public int RecordsTotal { get; set; }
    }
}
