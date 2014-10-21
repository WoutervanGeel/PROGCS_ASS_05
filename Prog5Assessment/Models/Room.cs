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
        public int MaxPersons { get; set; }
        public int MinPrice { get; set; }
    }
}