using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public List<CustomPrices> CustomPrices { get; set; }

        [Display(Name = "Maximal number of persons")]
        public int MaxPersons { get; set; }

        [Display(Name="Minimal price per night")]
        public decimal MinPrice { get; set; }
    }
}