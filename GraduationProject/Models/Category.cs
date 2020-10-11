using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraduationProject.Models
{
    public class Category
    {
        public int id { get; set; }

        [Required]
        [Display(Name="Job Category")]
        public string CategoryName{get; set;}

         [Required]
        [Display(Name="Job Description")]
        public string CategoryDescription{get; set;}

         public virtual ICollection<Job> Jobs { get; set; }

    }
}