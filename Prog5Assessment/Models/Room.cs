using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Display(Name = "Maximal number of persons: ")]
        public int MaxPersons { get; set; }
        [Display(Name="Minimal price per night: ")]
        public int MinPrice { get; set; }
    }
}