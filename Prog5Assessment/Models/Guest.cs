﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Infix")]
        public string Infix { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

    }
}