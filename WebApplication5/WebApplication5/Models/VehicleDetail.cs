using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class VehicleDetail 
    {
        
        public string id { get; set; }
        public string LotId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Trim { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public string PaymentDone { get; set; }
        public string IsPicked { get; set; }
        public string BuyerID { get; set; }
        public string Location { get; set; }
    }
}