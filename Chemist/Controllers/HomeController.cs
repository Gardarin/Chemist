using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using Chemist.Models;
using System.Web.Routing;

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
            return View();
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            //Chemist.Models.DbGenerate.AddItems(_chemistContext); 

            //User user = new Models.User();
            //user.IsAdmin = true;
            //user.Mail = "absd@mail.ru";
            //user.Password = "123456";
            //user.Name = "Igor";
            //_chemistContext.Users.Add(user);
            //_chemistContext.SaveChanges();

            IQueryable<Medicament> medicaments = _chemistContext.Medicaments;
            ViewBag.medicaments = medicaments;
            Basket basket;
            HttpCookie cookie = Request.Cookies.Get("BasketId");
            if (cookie == null)
            {
                cookie=new HttpCookie("BasketId",Session.SessionID);
                Response.SetCookie(cookie);
                return View();
            }
            int Id = 0;
            if (!int.TryParse(id, out Id))
            {
                return View();
            }

            string sesId = Request.Cookies.Get("BasketId").Value;

            basket = _chemistContext.Baskets.FirstOrDefault(x => x.SessionId == sesId);
            
            if(basket == null)
            {
                basket = new Basket();
                basket.SessionId = Request.Cookies.Get("BasketId").Value;
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
            ViewBag.Report = "Добавлено";
            return View();
        }

        [HttpPost]
        public string Buy(Item item)
        {

            var basket = _chemistContext.Baskets.First(x => x.SessionId == Request.Cookies.Get("BasketId").Value);
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

            if (Request.Cookies.Get("BasketId") == null)
            {
                return View();
            }
            string sesId = Request.Cookies.Get("BasketId").Value;

            Basket basket = _chemistContext.Baskets.Include("Items.Medicament").FirstOrDefault(x => x.SessionId == sesId);
            if (basket == null)
            {
                return View();
            }
            ViewBag.Message = "Your contact page.";
            ViewBag.Basket = basket;
            return View();
        }

        [HttpGet]
        public ActionResult CreateOrder()
        {
            if (Request.Cookies.Get("BasketId") == null)
            {
                return View();
            }

            string sesId = Request.Cookies.Get("BasketId").Value;

            Basket basket = _chemistContext.Baskets.Include("Items.Medicament").FirstOrDefault(x => x.SessionId == sesId);
            if (basket == null)
            {
                return View();
            }

            ViewBag.Basket = basket;
            ViewBag.AmountPrice = basket.Items.Sum(x=>(x.Medicament.Price*x.Count));

            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(Order order)
        {
            if (Request.Cookies.Get("BasketId") == null)
            {
                return View();
            }

            string sesId = Request.Cookies.Get("BasketId").Value;

            //order.Items = _chemistContext.Baskets.FirstOrDefault(x => x.SessionId == sesId).Items;

            Basket basket = basket = _chemistContext.Baskets.FirstOrDefault(x => x.SessionId == sesId);

            basket.SessionId = Request.Cookies.Get("BasketId").Value;
            basket.Items = new List<Item>();

            _chemistContext.Orders.Add(order);
            _chemistContext.SaveChanges();
            return Index();
        }

        [HttpGet]
        public ActionResult AdminPage()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AdminPage(AutenteficationAdmin aut)
        {
            User user = _chemistContext.Users.FirstOrDefault(x => (x.Mail == aut.Email && x.Password == aut.Password));
            if (user == null)
            {
                return View();
            }
            HttpCookie cookie = Request.Cookies.Get("AdminId");
            if (cookie == null)
            {
                cookie = new HttpCookie("AdminId", "true");
                Response.SetCookie(cookie);
                
                return RedirectToAction("Orders");
            }

            return RedirectToAction("Orders"); 
        }

        public ActionResult Orders()
        {
            HttpCookie cookie = Request.Cookies.Get("AdminId");
            if (cookie == null)
            {
                return RedirectToAction("Index");
            }
            if (cookie.Value != "true")
            {
                return RedirectToAction("Index");
            }

            ViewBag.Orders = _chemistContext.Orders.Include("Items").ToList();
            List<Order> or = _chemistContext.Orders.Include("Items").ToList();

            return View();
        }
    }
}