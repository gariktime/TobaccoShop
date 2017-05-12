using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.ListModels;
using TobaccoShop.BLL.Services;
using TobaccoShop.DAL.Interfaces;
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

        private IUnitOfWork db;
        private ProductService productService;

        public ProductsController(IUnitOfWork uow)
        {
            db = uow;
            productService = new ProductService(db);
        }

        public ActionResult Hookahs()
        {
            HookahListModel hlm = new HookahListModel(db);
            return View(hlm);
        }

        public async Task<ActionResult> HookahFilter(HookahListViewModel hlvm)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var hookahs = await productService.GetHookahsAsync(hlvm.minPrice, hlvm.maxPrice, hlvm.minHeight, hlvm.maxHeight, hlvm.SelectedMarks);
                    return PartialView("_ProductList", hookahs);
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
