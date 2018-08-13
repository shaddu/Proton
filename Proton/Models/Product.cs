using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proton.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Prodcut Code is required")]
        [Remote(action: "VerifyProductCode", controller: "Product", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "Prodcut Code already in use.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Photo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater then 0")]

        public Nullable<int> Price { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    }
}