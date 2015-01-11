using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Reservation
    {

        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int PriceTotal { get; set; }
        [Required]
        public string InvoiceAddress { get; set; }
        [Required]
        public string InvoiceCity { get; set; }
        [Required]
        public string InvoicePostal { get; set; }
        [Required]
        public int BankAccount { get; set; }
        public int VoucherId { set; get; }
        public int Guests { get; set; }
    }
}