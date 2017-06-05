﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.Models.ProductListModels;

namespace TobaccoShop.Controllers
{
    public class ProductsController : Controller
    {
        private IProductService productService;

        public ProductsController(IProductService prService)
        {
            productService = prService;
        }

        [NonAction]
        public JsonResult AddToCart(Guid id)
        {
            List<OrderedProductDTO> products = (List<OrderedProductDTO>)Session["Cart"];
            if (products == null)
            {
                products = new List<OrderedProductDTO>();
                var product = new OrderedProductDTO() { Id = Guid.NewGuid(), ProductId = id };
                products.Add(product);
                Session["Cart"] = products;
            }
            else
            {
                var product = new OrderedProductDTO() { Id = Guid.NewGuid(), ProductId = id };
                products.Add(product);
            }
            int cart_count = products.Count;
            return Json(cart_count, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Hookahs()
        {
            ViewData["Products"] = await productService.GetHookahsAsync();
            HookahListViewModel hlvm = new HookahListViewModel();
            var hookahProps = await productService.GetHookahProperties();
            hlvm.MinPrice = hookahProps.Item1;
            hlvm.MaxPrice = hookahProps.Item2;
            hlvm.MinHeight = hookahProps.Item3;
            hlvm.MaxHeight = hookahProps.Item4;
            hlvm.Marks = hookahProps.Item5;
            hlvm.Countries = hookahProps.Item6;
            return View(hlvm);
        }

        [HttpPost]
        public async Task<ActionResult> HookahFilter(HookahListViewModel hlvm)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var hookahs = await productService.GetHookahsAsync(hlvm.MinPrice, hlvm.MaxPrice, hlvm.MinHeight, hlvm.MaxHeight, hlvm.SelectedMarks, hlvm.SelectedCountries);
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
