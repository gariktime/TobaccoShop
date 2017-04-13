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
            ViewBag.Marks = db.Hookahs.Select(p => p.Mark).Distinct().ToList();
            return View(await db.Hookahs.ToListAsync());
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