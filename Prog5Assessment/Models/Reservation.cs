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
        public Movie PlayedMovie { get; set; }
        public int PriceTotal { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceCity { get; set; }
        public string InvoicePostal { get; set; }
        public int BankAccount { get; set; }
        public int Discount { set; get; }
    }
}