using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSBM.Web.Models
{
    public class EmailViewModel
    {   
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="Emial is not valid.")]
        public string ToEmail { get; set; }

        [EmailAddress(ErrorMessage = "Emial is not valid.")]
        public string CC { get; set; }

        [EmailAddress(ErrorMessage = "Emial is not valid.")]
        public string BCC { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Body")]
        public string Body { get; set; }

        [Display(Name = "Attachments")]
        public List<string> Attachments { get; set; }

        public string UserName { get; set; }
    }
}
