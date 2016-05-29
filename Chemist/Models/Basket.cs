using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chemist.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}