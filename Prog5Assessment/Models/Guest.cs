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
        public string FirstName { get; set; }
        public string Infix { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string Address { get; set; }
        public string Postalcode { get; set; }
        public string Location { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}