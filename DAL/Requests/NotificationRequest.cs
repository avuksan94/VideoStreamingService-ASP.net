using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Requests
{
    public class NotificationRequest
    {
        [Required]
        [EmailAddress]
        public string? ReceiverEmail { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
