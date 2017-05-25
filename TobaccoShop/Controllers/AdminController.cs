using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Services;
using TobaccoShop.DAL.Interfaces;
using TobaccoShop.Models;
using TobaccoShop.Models.ProductModels;

namespace TobaccoShop.Controllers
{
    public class AdminController : Controller
    {
        private IProductService productService;

        public AdminController(IProductService prService)
        {
            productService = prService;
        }

        #region Добавление/редактирование/удаление товаров

        //добавление нового товара
        public ActionResult AddProduct()
        {
            var st = from ProductType pt in Enum.GetValues(typeof(ProductType))
                     select new { ID = (int)pt, Name = pt.ToString() };
            SelectList types = new SelectList(st, "ID", "Name", ProductType.Hookah);
            ViewBag.Types = types;
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductType productType)
        {
            if (Request.IsAjaxRequest())
            {
                if (productType == ProductType.Hookah)
                    return PartialView("_AddHookah");
                else if (productType == ProductType.HookahTobacco)
                    return PartialView("_AddHookahTobacco");
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //редактирование товара
        public async Task<ActionResult> Edit(Guid id)
        {
            var product = await productService.FindByIdAsync(id);
            ProductType type = productService.GetProductType(product);
            if (type == ProductType.Hookah)
            {
                var hookah = productService.ProductAsHookah(product);
                HookahViewModel hvm = new HookahViewModel() { ProductId = hookah.ProductId, Mark = hookah.Mark, Model = hookah.Model, Country = hookah.Country, Description = hookah.Description, Available = hookah.Available, Price = hookah.Price, Height = hookah.Height, Image = hookah.Image };
                return PartialView("_AddHookah", hvm);
            }
            return RedirectToAction("Products");
        }

        //удаление товара
        public async Task<ActionResult> Remove(Guid id)
        {
            await productService.RemoveProduct(id);
            return RedirectToAction("Products");
        }

        //список всех продуктов
        public ActionResult Products()
        {
            var products = productService.GetProducts();
            return View(products);
        }

        [HttpPost]
        public async Task<ActionResult> Products(string searchQuery)
        {
            if (Request.IsAjaxRequest())
            {
                var products = await productService.GetProductsAsync(p => p.Mark.Contains(searchQuery) ||
                                                                          p.Model.Contains(searchQuery));
                return PartialView("_ProductList", products);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddHookah([Bind(Exclude = "Image")] HookahViewModel hvm, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                }

                //создание DTO из ViewModel
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

                if (hvm.ProductId == null) //добавление нового продукта
                {
                    await productService.AddHookah(hookahDto);
                    return RedirectToAction("AddProduct");
                }
                else //изменение существующего
                {
                    await productService.EditHookah(hvm.ProductId, hookahDto);
                    return RedirectToAction("Products");
                }
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
