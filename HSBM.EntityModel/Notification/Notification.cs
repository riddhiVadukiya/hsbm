using BLToolkit.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Notification
{
    public class Notification
    {
        public long Id { get; set; }

        public long UserType { get; set; }

        public int NotificationType { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [MapIgnore]
        public virtual List<NotificationDetail> NotificationDetails { get; set; }
    }
}
