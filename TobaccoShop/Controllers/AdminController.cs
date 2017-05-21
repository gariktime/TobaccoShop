using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Services;
using TobaccoShop.DAL.Interfaces;
using TobaccoShop.Models;
using TobaccoShop.Models.ProductModels;

namespace TobaccoShop.Controllers
{
    public class AdminController : Controller
    {
        private IUnitOfWork db;
        private ProductService productService;

        public AdminController(IUnitOfWork uow)
        {
            db = uow;
            productService = new ProductService(db);
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Добавление новых товаров

        public ActionResult AddProduct()
        {
            List<string> list = new List<string>() { "Кальян", "Табак для кальяна" };
            SelectList types = new SelectList(list);
            ViewBag.Types = types;
            ViewBag.HookahModel = new HookahViewModel();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(string productType)
        {
            if (Request.IsAjaxRequest())
            {
                if (productType == "Кальян")
                    return PartialView("_AddHookah");
                else if (productType == "Табак для кальяна")
                    return PartialView("_AddHookahTobacco");
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddHookah(HookahViewModel hvm, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                HookahDTO hookahDto = new HookahDTO
                {
                    Mark = hvm.Mark,
                    Model = hvm.Model,
                    Height = hvm.Height,
                    Price = hvm.Price,
                    Available = hvm.Available,
                    Country = hvm.Country,
                    Description = hvm.Description,
                    Image = imageData
                };
                await productService.AddHookah(hookahDto);
                return RedirectToAction("AddProduct");
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> AddHookahTobacco(HookahTobaccoViewModel htvm)
        {
            if (ModelState.IsValid)
            {
                HookahTobaccoDTO tobacco = new HookahTobaccoDTO();
                await productService.AddHookahTobacco(tobacco);
                return RedirectToAction("AddProduct");
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

    }
}
