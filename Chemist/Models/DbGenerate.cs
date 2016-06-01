using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Drawing;

namespace Chemist.Models
{
    public class ChemistDbInitializer: DropCreateDatabaseAlways<ChemistContext>
    {
        public static void AddItems(ChemistContext chemistContext)
        {
            Medicament med = new Medicament();
            Bitmap bitmap = new Bitmap("1.bmp");
            med.Name = "QQQ1000";
            med.LatinName = "qqqkilo";
            med.ByPrescription = true;
            med.Code = "1232345";
            med.Description = "la la la";
            med.Price = 1000;
            med.SetPicture(bitmap);
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            bitmap = new Bitmap("2.bmp");
            med.Name = "Ciclofosfamin";
            med.LatinName = "lat ciclo";
            med.ByPrescription = false;
            med.Code = "666";
            med.Description = "description 3";
            med.Price = 1500;
            med.SetPicture(bitmap);
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            bitmap = new Bitmap("3.bmp");
            med.Name = "Etanol";
            med.LatinName = "etan";
            med.ByPrescription = true;
            med.Code = "777";
            med.Description = "spirt";
            med.Price = 2000;
            med.SetPicture(bitmap);
            chemistContext.Medicaments.Add(med);

            chemistContext.SaveChanges();
        }

        protected override void Seed(ChemistContext chemistContext)
        {
            Medicament med = new Medicament();
            Bitmap bitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\1.bmp");
            med.Name = "QQQ1000";
            med.LatinName = "qqqkilo";
            med.ByPrescription = true;
            med.Code = "1232345";
            med.Description = "la la la";
            med.Price = 1000;
            med.Count = 100;
            med.SetPicture(bitmap);
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            bitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\2.bmp");
            med.Name = "Ciclofosfamin";
            med.LatinName = "lat ciclo";
            med.ByPrescription = false;
            med.Code = "666";
            med.Description = "description 3";
            med.Price = 1500;
            med.Count = 10;
            med.SetPicture(bitmap);
            chemistContext.Medicaments.Add(med);

            med = new Medicament();
            bitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\3.bmp");
            med.Name = "Etanol";
            med.LatinName = "etan";
            med.ByPrescription = true;
            med.Code = "777";
            med.Description = "spirt";
            med.Price = 2000;
            med.Count = 35;
            med.SetPicture(bitmap);
            chemistContext.Medicaments.Add(med);

            User user = new Models.User();
            user.IsAdmin = true;
            user.Mail = "abcd@mail.ru";
            user.Password = "123456";
            user.Name = "Igor";
            user.Surname = "Sol";
            user.PhoneNumber = "666";
            user.Age=21;
            user.Address="Mars";
            user.CurentSession = "igorffadminpp";
            chemistContext.Users.Add(user);

            base.Seed(chemistContext);
        }
    }
}