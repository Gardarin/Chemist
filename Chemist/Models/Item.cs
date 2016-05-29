using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chemist.Models
{
    public class Item
    {
        public int Id { set; get; }
        public Medicament Medicament { set; get; }
        public int Count { set; get; }
    }
}