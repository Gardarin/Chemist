using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using Chemist.Models;

namespace Chemist.Controllers
{
    public class HomeController : Controller
    {
        ChemistContext _chemistContext = new ChemistContext();

        public ActionResult Index()
        {
            //Chemist.Models.DbGenerate.AddItems(_chemistContext); 
            IQueryable<Medicament> medicaments = _chemistContext.Medicaments;

            ViewBag.medicaments = medicaments;
            ViewBag.Report = "";
            
            //Bitmap b = new Bitmap("");

            //Session.SessionID   !!!!!!!!!!!!!!

            //b.Save(,System.Drawing.Imaging.ImageFormat.Bmp);

            return View();
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            //Chemist.Models.DbGenerate.AddItems(_chemistContext); 
            IQueryable<Medicament> medicaments = _chemistContext.Medicaments;
            ViewBag.medicaments = medicaments;
            ViewBag.Report = "Добавлено";
            int Id = 0;
            if (!int.TryParse(id, out Id))
            {
                return View();
            }
            Basket basket;
            if (_chemistContext.Baskets.Count() > 0)
            {
                basket = _chemistContext.Baskets.First(x => x.SessionId == Request.UserHostAddress);
            }
            else
            {
                basket = new Basket();
                basket.SessionId = Request.UserHostAddress;
                basket.Items = new List<Item>();
                _chemistContext.Baskets.Add(basket);
            }
            Item item = new Item();
            item.Count = 1;
            item.Medicament = _chemistContext.Medicaments.Find(Id);
            if (item.Medicament != null)
            {
                basket.Items.Add(item);
            }
            _chemistContext.SaveChanges();
            return View();
        }

        [HttpPost]
        public string Buy(Item item)
        {

            var basket = _chemistContext.Baskets.First(x => x.SessionId == Request.UserHostAddress);
            if (basket == null)
            {
                basket = new Basket();
                basket.SessionId = Request.UserHostAddress;
                basket.Items = new List<Item>();
                _chemistContext.Baskets.Add(basket);
            }
            basket.Items.Add(item);
            _chemistContext.SaveChanges();
            return "Добавлено";
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";



            return View();
        }

        public ActionResult Contact()
        {
            if (_chemistContext.Baskets.Count() == 0)
            {
                return View();
            }
            Basket basket = _chemistContext.Baskets.Include("Items.Medicament").FirstOrDefault(x => x.SessionId == Request.UserHostAddress);
            if (basket == null)
            {
                return View();
            }

            ViewBag.Message = "Your contact page.";

            ViewBag.Basket = basket;

            return View();
        }
    }
}