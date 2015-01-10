using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public Room Room { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }
}