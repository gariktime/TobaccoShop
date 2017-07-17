using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.Models;
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddToCart(Guid productId)
        {
            List<OrderedProductDTO> products = (List<OrderedProductDTO>)Session["Cart"];
            ProductDTO product = await productService.FindByIdAsync(productId);

            if (product != null)
            {
                OrderedProductDTO cart_product = new OrderedProductDTO() { ProductId = product.ProductId, Quantity = 1, Price = product.Price, MarkModel = product.Mark + " " + product.Model };

                if (products == null) //если товары ещё не добавлялись то создаём корзину и добавляем выбранный продукт
                {
                    products = new List<OrderedProductDTO>();
                    products.Add(cart_product);
                    Session["Cart"] = products;
                }
                else //добавляем товар в корзину
                {
                    //если выбранного товара ещё нет в корзине
                    if (!products.Exists(p => p.ProductId == product.ProductId))
                        products.Add(cart_product);
                }
            }
            return PartialView("_CartMenu");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Item(Guid? id)
        {
            if (id != null)
            {
                var (product, productType) = await productService.GetProductParamsAsync((Guid)id);
                if (product != null)
                    return View(product);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string currUserId = User.Identity.GetUserId();
                CommentDTO commentDto = new CommentDTO()
                {
                    ProductId = model.ProductId,
                    Text = model.Text,
                    UserId = currUserId
                };

                var result = await productService.AddComment(commentDto);
                if (result.Succeeded == true)
                    return RedirectToAction("Item", "Products", new { id = model.ProductId });
                else
                    return PartialView("_AddComment", model);
            }
            else
                return PartialView("_AddComment", model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Hookahs()
        {
            ViewData["Products"] = await productService.GetHookahsAsync();
            HookahListViewModel hlvm = new HookahListViewModel();
            var (minPrice, maxPrice, minHeight, maxHeight, marks, countries) = await productService.GetHookahProperties();
            hlvm.MinPrice = minPrice;
            hlvm.MaxPrice = maxPrice;
            hlvm.MinHeight = minHeight;
            hlvm.MaxHeight = maxHeight;
            hlvm.Marks = marks;
            hlvm.Countries = countries;
            return View(hlvm);
        }

        [AllowAnonymous]
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

        protected override void Dispose(bool disposing)
        {
            productService.Dispose();
            base.Dispose(disposing);
        }
    }
}
