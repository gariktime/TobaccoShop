using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TobaccoShop.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Cart()
        {
            return View();
        }
    }
}
