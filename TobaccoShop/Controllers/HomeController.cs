using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.Models;
using System.Reflection;

namespace TobaccoShop.Controllers
{
    public class HomeController : Controller
    {
        ProductContext context = new ProductContext();

        public ActionResult Index()
        {
            return View();
        }
    }
}