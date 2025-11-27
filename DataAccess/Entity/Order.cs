using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public  string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
