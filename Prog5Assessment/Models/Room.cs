using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Room
    {
        public static Room[] rooms;

        public Room()
        {
            rooms = new Room[12];
        }
        [Key]
        public int Id { get; set; }

        [Display(Name = "Maximal number of persons")]
        public int MaxPersons { get; set; }

        [Display(Name="Minimal price per night")]
        public decimal MinPrice { get; set; }
    }
}