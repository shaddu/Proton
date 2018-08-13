using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    }
}