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
            IQueryable<Medicament> medicaments = _chemistContext.Medicaments;
            ViewBag.medicaments = medicaments;
            ViewBag.Report = "";
            return View();
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
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

            basket = _chemistContext.Baskets.Include("Items.Medicament").FirstOrDefault(x => x.SessionId == sesId);
            
            if(basket == null)
            {
                basket = new Basket();
                basket.SessionId = Request.Cookies.Get("BasketId").Value;
                basket.Items = new List<Item>();
                _chemistContext.Baskets.Add(basket);
            }

            basket.Items.FirstOrDefault(x => x.Medicament.Id == Id);


            Item item = basket.Items.FirstOrDefault(x => x.Medicament.Id == Id);
            if (item == null)
            {
                item = new Item();
                item.Count = 1;
                item.Medicament = _chemistContext.Medicaments.Find(Id);
            }
            else
            {
                item.Count++;
            }
            
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

        [HttpGet]
        public ActionResult Contact(string id)
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

            int Id = 0;
            if (int.TryParse(id, out Id))
            {
                Item item = basket.Items.FirstOrDefault(x => x.Id == Id);
                if (item.Count > 1)
                {
                    item.Count--;
                }
                else
                {
                    basket.Items.Remove(basket.Items.FirstOrDefault(x => x.Id == Id));
                }
                _chemistContext.SaveChanges();
            }

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
            Basket basket = _chemistContext.Baskets.Include("Items").FirstOrDefault(x => x.SessionId == sesId);

            basket.SessionId = Request.Cookies.Get("BasketId").Value;
            order.Items=new List<Item>();
            foreach(var i in basket.Items)
            {
                order.Items.Add(i);
            }
            basket.Items.Clear();
            order.Status = "isnt comit";
            _chemistContext.Orders.Add(order);
            _chemistContext.SaveChanges();
            return RedirectToAction("Index"); ;
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

        [HttpGet]
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

            ViewBag.Orders = _chemistContext.Orders.Include("Items.Medicament").ToList();

            return View();
        }
        
        public ActionResult OrdersDel(int id)
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

            var orders = _chemistContext.Orders.Include("Items").FirstOrDefault(x => x.Id == id);
            if (orders != null)
            {
                _chemistContext.Orders.Remove(orders);
                _chemistContext.SaveChanges();
            }

            return RedirectToAction("Orders");
        }

        public ActionResult OrdersComit(int id)
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

            var order = _chemistContext.Orders.Include("Items.Medicament").FirstOrDefault(x => x.Id == id);
            if (order != null)
            {
                order.Status = "Comit";

                foreach (var i in order.Items)
                {
                    i.Medicament.Count -= i.Count;
                }

                _chemistContext.SaveChanges();
            }

            return RedirectToAction("Orders");
        }
    }
}