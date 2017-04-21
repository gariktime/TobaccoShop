using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.DAL.Repositories;
using TobaccoShop.Models;

namespace TobaccoShop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        EFUnitOfWork uow;

        public ProductsController()
        {
            //uow = new EFUnitOfWork();
        }

        public async Task<ActionResult> Hookahs()
        {
            HookahListViewModel hlvm = new HookahListViewModel();
            hlvm.Products = await uow.Products.GetHookahs().ToListAsync();
            hlvm.Marks = await uow.Products.GetMarksAsync("Hookah");
            hlvm.minPrice = await uow.Products.GetMinPriceAsync();
            hlvm.maxPrice = await uow.Products.GetMaxPriceAsync();
            hlvm.minHeight = await uow.Products.GetHookahs().Select(c => c.Height).MinAsync();
            hlvm.maxHeight = await uow.Products.GetHookahs().Select(c => c.Height).MaxAsync();

            return View(hlvm);
        }

        public async Task<ActionResult> HookahFilter(HookahListViewModel hlvm)
        {
            return PartialView("_ProductList", null);
        }
    }
}