using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace refactor_me.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}