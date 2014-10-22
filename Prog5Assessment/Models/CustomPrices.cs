using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class CustomPrices
    {
        [Key]
        public int Id { get; set; }

        public int Room_Id { get; set; }

        [Display(Name = "Start date")]
        public DateTime DateStart { get; set; }

        [Display(Name = "End date")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

    }
}