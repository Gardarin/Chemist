using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chemist.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyersName { get; set; }
        public string ShippingAddress { get; set; }
        public string Email { get; set; }
        public string PhoneNamber { get; set; }
        public int AmountPrice { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}