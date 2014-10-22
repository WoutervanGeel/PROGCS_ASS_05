using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class Booking
    {

        [Key]
        public int Id { get; set; }
        public Room BookedRoom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Guests { get; set; }
        public float Price { get; set; }
        public string AddressStreetNumber { get; set; }
        public string AddressPostal { get; set; }
        public string AddressStreet { get; set; }
        public string AddressPlace { get; set; }
        public string AddressCountry { get; set; }
        public string InvoiceAddressStreetNumber { get; set; }
        public string InvoiceAddressPostal { get; set; }
        public string InvoiceAddressStreet { get; set; }
        public string InvoiceAddressPlace { get; set; }
        public string InvoiceAddressCountry { get; set; }

        public int AccountNr { get; set; }
    }
}