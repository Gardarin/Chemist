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

            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie == null)
            {
                return View();
            }

            User user = _chemistContext.Users.FirstOrDefault(x => x.CurentSession == cookie.Value);
            ViewBag.User = user;

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
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie == null)
            {
                return View();
            }

            User user = _chemistContext.Users.FirstOrDefault(x => x.CurentSession == cookie.Value);
            if (user == null)
            {
                return View();
            }

            return RedirectToAction("UserPage"); 
        }

        [HttpPost]
        public ActionResult AdminPage(AutenteficationAdmin aut)
        {
            User user = _chemistContext.Users.FirstOrDefault(x => (x.Mail == aut.Email && x.Password == aut.Password));
            if (user == null)
            {
                return View();
            }
            HttpCookie cookie = Request.Cookies.Get("UserId");
            cookie = new HttpCookie("UserId", user.CurentSession);
            Response.SetCookie(cookie);

            return RedirectToAction("UserPage"); 
        }

        [HttpGet]
        public ActionResult UserPage()
        {
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie == null)
            {
                return RedirectToAction("Index");
            }
            
            User user = _chemistContext.Users.FirstOrDefault(x => x.CurentSession == cookie.Value);
            if (user == null)
            {
                return RedirectToAction("Index");
            }


            ViewBag.User = user;
            return View();
        }

        [HttpGet]
        public ActionResult Orders()
        {
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie == null)
            {
                return RedirectToAction("Index");
            }

            User user = _chemistContext.Users.FirstOrDefault(x=>x.CurentSession == cookie.Value);

            if (!user.IsAdmin)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Orders = _chemistContext.Orders.Include("Items.Medicament").ToList();

            return View();
        }
        
        public ActionResult OrdersDel(int id)
        {
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie == null)
            {
                return RedirectToAction("Index");
            }
            User user = _chemistContext.Users.FirstOrDefault(x => x.CurentSession == cookie.Value);

            if (!user.IsAdmin)
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
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (cookie == null)
            {
                return RedirectToAction("Index");
            }
            User user = _chemistContext.Users.FirstOrDefault(x => x.CurentSession == cookie.Value);

            if (!user.IsAdmin)
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

        public ActionResult Exit()
        {
            HttpCookie cookie = Response.Cookies.Get("UserId");
            if (cookie == null)
            {
                return RedirectToAction("Index");
            }
            cookie.Value = "-1";
            Response.Cookies.Set(cookie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(User user)
        {
            user.CurentSession = Session.SessionID + user.GetHashCode();
            user.IsAdmin = false;
            _chemistContext.Users.Add(user);
            _chemistContext.SaveChanges();

            HttpCookie cookie = new HttpCookie("UserId", user.CurentSession);
            Response.SetCookie(cookie);

            return RedirectToAction("UserPage"); ;
        }


        [HttpGet]
        public ActionResult CreateMedicament()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMedicament(Medicament medicament)
        {
            if (medicament != null)
            {
                //byte[] buffer=new byte[Request.Files.Get(medicament.Image).InputStream.Length];
                //Request.Files.Get(medicament.Image).InputStream.Read(buffer,1,buffer.Length);
                medicament.Picture = new byte[50];
                _chemistContext.Medicaments.Add(medicament);
                _chemistContext.SaveChanges();
            }

            return View();
        }
    }
}