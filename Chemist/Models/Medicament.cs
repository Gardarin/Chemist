using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chemist.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string Code { get; set; }
        public bool ByPrescription { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public byte[] Picture { get; set; }
    }
}