using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u_market.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public Store store { get; set; }
    }
}