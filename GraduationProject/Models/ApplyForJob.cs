using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GraduationProject.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }
        [DisplayName("Apply Date")]
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }

        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
