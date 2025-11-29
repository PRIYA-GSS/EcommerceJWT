using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateProduct
    {
    
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

    }
}
