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
        //private ProductContext db = new ProductContext();

        //public async Task<ActionResult> List()
        //{
        //    HookahListViewModel hlvm = new HookahListViewModel();
        //    hlvm.Products = await db.Hookahs.ToListAsync();
        //    hlvm.minPrice = await db.Hookahs.Select(p => p.Price).MinAsync();
        //    hlvm.maxPrice = await db.Hookahs.Select(p => p.Price).MaxAsync();
        //    hlvm.minHeight = await db.Hookahs.Select(p => p.Height).MinAsync();
        //    hlvm.maxHeight = await db.Hookahs.Select(p => p.Height).MaxAsync();
        //    hlvm.Marks = await db.Hookahs.Select(p => p.Mark).ToListAsync();

        //    return View(hlvm);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ProductFilter(HookahListViewModel hlvm)
        //{
        //    if (hlvm.minPrice < 1)
        //        ModelState.AddModelError("minPrice", "Минимальная цена не может быть отрицательной");

        //    if (Request.IsAjaxRequest())
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            IQueryable<Hookah> seq = null;
        //            if (hlvm.SelectedMarks == null)
        //                seq = db.Hookahs.Where(p => p.Price >= hlvm.minPrice &&
        //                                            p.Price <= hlvm.maxPrice &&
        //                                            p.Height >= hlvm.minHeight &&
        //                                            p.Height <= hlvm.maxHeight);
        //            else
        //                seq = db.Hookahs.Where(p => p.Price >= hlvm.minPrice &&
        //                                            p.Price <= hlvm.maxPrice &&
        //                                            p.Height >= hlvm.minHeight &&
        //                                            p.Height <= hlvm.maxHeight &&
        //                                            hlvm.SelectedMarks.Contains(p.Mark));

        //            return PartialView("_ProductList", await seq.ToListAsync());
        //        }
        //        else
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    else
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}

        //public async Task<ActionResult> Item(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Hookah hookah = await db.Hookahs.FindAsync(id);
        //    if (hookah == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hookah);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}