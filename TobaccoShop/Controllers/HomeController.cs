using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.Models;

namespace TobaccoShop.Controllers
{
    public class HomeController : Controller
    {
        ProductContext context = new ProductContext();

        public ActionResult Index()
        {
            IEnumerable<Product> products = context.Products;
            Product p = context.Products.Find(1);
            ViewBag.Products = products;
            return View();
        }

    }
}