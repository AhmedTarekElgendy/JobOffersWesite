using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace GraduationProject.Models
{
    public class Profile
    {
        public int id { get; set; }
        public string UserId { get; set; }

        [DisplayName("Photo")]
        public string img { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [AllowHtml]
        [DisplayName("Bio")]
        public string Bio { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("E-Mail")]
        public string Mail { get; set; }

        [DisplayName("Other Links")]
        public string OtherLinks { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? Birth { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Marital Status")]
        public string State { get; set; }
        
        [DisplayName("Military Status")]
        public string Military { get; set; }

        [DisplayName("Education")]
        public string Education { get; set; }

        [DisplayName("Job Title")]
        public string Title { get; set; }

        [DisplayName("Year Of Experience")]
        public int? Experience { get; set; }

        [DisplayName("CV")]
        public string CV { get; set; }

        public virtual ApplicationUser user { get; set; }
    }
}