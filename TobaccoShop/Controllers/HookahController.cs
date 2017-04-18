using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.Models;
using System.Net;
using System.Data.Entity;

namespace TobaccoShop.Controllers
{
    public class HookahController : Controller
    {
        private ProductContext db = new ProductContext();

        public async Task<ActionResult> List()
        {
            HookahListViewModel hlvm = new HookahListViewModel();
            hlvm.Products = await db.Hookahs.ToListAsync();
            hlvm.minPrice = await db.Hookahs.Select(p => p.Price).MinAsync();
            hlvm.maxPrice = await db.Hookahs.Select(p => p.Price).MaxAsync();
            hlvm.minHeight = await db.Hookahs.Select(p => p.Height).MinAsync();
            hlvm.maxHeight = await db.Hookahs.Select(p => p.Height).MaxAsync();
            hlvm.Marks = new List<MarkItem>();
            var mrks = db.Hookahs.Select(p => p.Mark).ToList();
            for (int i = 0; i < mrks.Count(); i++)
            {
                hlvm.Marks.Add(new MarkItem(mrks[i], false));
            }

            return View(hlvm);
        }

        [HttpPost]
        public ActionResult ProductList(HookahListViewModel hlvm)
        {
            var tt = hlvm;
            if (Request.IsAjaxRequest())
            {
                
                int minPrice = int.Parse( Request.Form.GetValues("price_min")[0]);
                var k = 0;
                return PartialView();
            }
            return PartialView("_ProductList");
        }

        public async Task<ActionResult> Item(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hookah hookah = await db.Hookahs.FindAsync(id);
            if (hookah == null)
            {
                return HttpNotFound();
            }
            return View(hookah);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}