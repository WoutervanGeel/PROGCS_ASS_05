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
        public int Id { get; set; }

        public int RoomId { get; set; }

        [Column(TypeName="DateTime")]
        public DateTime DateStart { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime DateEnd { get; set; }

    }
}