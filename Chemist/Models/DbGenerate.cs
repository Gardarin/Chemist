using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Chemist.Models
{
    public class ChemistDbInitializer: DropCreateDatabaseAlways<ChemistContext>
    {
        public static void AddItems(ChemistContext chemistContext)
        {
            Medicament med = new Medicament();
            med.Name = "QQQ1000";
            med.LatinName = "qqqkilo";
            med.ByPrescription = true;
            med.Code = "1232345";
            med.Description = "la la la";
            med.Price = 1000;
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            med.Name = "Ciclofosfamin";
            med.LatinName = "lat ciclo";
            med.ByPrescription = false;
            med.Code = "666";
            med.Description = "description 3";
            med.Price = 1500;
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            med.Name = "Etanol";
            med.LatinName = "etan";
            med.ByPrescription = true;
            med.Code = "777";
            med.Description = "spirt";
            med.Price = 2000;
            chemistContext.Medicaments.Add(med);

            chemistContext.SaveChanges();
        }

        protected override void Seed(ChemistContext chemistContext)
        {
            Medicament med = new Medicament();
            med.Name = "QQQ1000";
            med.LatinName = "qqqkilo";
            med.ByPrescription = true;
            med.Code = "1232345";
            med.Description = "la la la";
            med.Price = 1000;
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            med.Name = "Ciclofosfamin";
            med.LatinName = "lat ciclo";
            med.ByPrescription = false;
            med.Code = "666";
            med.Description = "description 3";
            med.Price = 1500;
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            med.Name = "Etanol";
            med.LatinName = "etan";
            med.ByPrescription = true;
            med.Code = "777";
            med.Description = "spirt";
            med.Price = 2000;
            chemistContext.Medicaments.Add(med);

            User user = new Models.User();
            user.IsAdmin = true;
            user.Mail = "abcd@mail.ru";
            user.Password = "123456";
            user.Name = "Igor";
            chemistContext.Users.Add(user);

            base.Seed(chemistContext);
        }
    }
}