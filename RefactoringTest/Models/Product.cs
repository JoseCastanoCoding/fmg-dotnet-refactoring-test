using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace refactor_me.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}