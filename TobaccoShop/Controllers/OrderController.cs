using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.Models;

namespace TobaccoShop.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {

        }

        #region Работа с корзиной

        public ActionResult Cart()
        {
            CartViewModel cvm = new CartViewModel();
            var products = (List<OrderedProductDTO>)Session["Cart"];
            cvm.Products = (products == null || products.Count == 0) ? null : products;
            cvm.TotalPrice = (products == null || products.Count == 0) ? 0 : products.Sum(p => p.LinePrice);
            return View(cvm);
        }

        //увеличение количества выбранного товара в корзине
        [HttpPost]
        public ActionResult IncreaseItem(Guid id)
        {
            if (Request.IsAjaxRequest())
            {
                var products = (List<OrderedProductDTO>)Session["Cart"];
                var product = products?.FirstOrDefault(p => p.Id == id);
                if (product != null)
                    product.Quantity += 1;
                CartViewModel cvm = new CartViewModel() { Products = products, TotalPrice = products.Sum(p => p.LinePrice) };
                return PartialView("_CartTable", cvm);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //уменьшение количества выбранного товара в корзине
        [HttpPost]
        public ActionResult DecreaseItem(Guid id)
        {
            if (Request.IsAjaxRequest())
            {
                var products = (List<OrderedProductDTO>)Session["Cart"];
                var product = products?.FirstOrDefault(p => p.Id == id);
                if (product != null)
                    product.Quantity = (product.Quantity == 1) ? 1 : product.Quantity - 1;
                CartViewModel cvm = new CartViewModel() { Products = products, TotalPrice = products.Sum(p => p.LinePrice) };
                return PartialView("_CartTable", cvm);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //удаление выбранного товара из корзины
        [HttpPost]
        public ActionResult DeleteItem(Guid id)
        {
            if (Request.IsAjaxRequest())
            {
                var products = (List<OrderedProductDTO>)Session["Cart"];
                var product = products?.FirstOrDefault(p => p.Id == id);
                if (product != null)
                    products.Remove(product);
                CartViewModel cvm = new CartViewModel() { Products = products, TotalPrice = products.Sum(p => p.LinePrice) };
                return PartialView("_CartTable", cvm);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}
