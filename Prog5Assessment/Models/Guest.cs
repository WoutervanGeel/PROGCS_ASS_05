using System;
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

        public Reservation MadeReservation { get; set; }

        [Display(Name = "First name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Insertion")]
        public string Insertion { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}