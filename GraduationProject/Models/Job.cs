using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraduationProject.Models
{
    public class Job
    {
        public int Id { get; set; }

        [DisplayName("Job Name")]
        public string JobTitle { get; set; }
        [AllowHtml]
        [DisplayName("Required Skills")]
        public string JobContent { get; set; }

        [DisplayName("Job Image")]
        public string JobImage { get; set; }

        [DisplayName("Job Type")]
        public int CategoryId { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }


        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Military")]
        public string Military { get; set; }

        public string UserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser user { get; set; }

    }
}