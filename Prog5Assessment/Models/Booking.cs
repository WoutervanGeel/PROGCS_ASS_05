using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Booking
    {
        public Room BookedRoom { get; set; }
        public List<Guest> guests { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InvoiceAddress { get; set; }
        public int AccountNr { get; set; }
    }
}