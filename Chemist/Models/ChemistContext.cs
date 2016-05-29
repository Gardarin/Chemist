using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Chemist.Models
{
    public class ChemistContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Basket> Baskets { get; set; }
    }
}