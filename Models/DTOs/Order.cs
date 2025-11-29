using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CreateOrder
    {
       
        public DateTime Date { get; set; }
        public string UserId { get; set; }

    }
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public IList<string> Products { get; set; } = new List<string>();
    }
}
