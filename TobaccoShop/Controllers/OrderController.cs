﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.Models;

namespace TobaccoShop.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService orderService;

        private IUserService UserService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }

        public OrderController(IOrderService oService)
        {
            orderService = oService;
        }

        #region Работа с корзиной

        //корзина покупок
        [AllowAnonymous]
        public ActionResult Cart()
        {
            CartViewModel cvm = new CartViewModel();
            var products = (List<OrderedProductDTO>)Session["Cart"];
            cvm.Products = (products == null || products.Count == 0) ? null : products;
            cvm.TotalPrice = (products == null || products.Count == 0) ? 0 : products.Sum(p => p.LinePrice);
            return View(cvm);
        }

        //увеличение количества выбранного товара в корзине
        [AllowAnonymous]
        [HttpPost]
        public ActionResult IncreaseItem(Guid productId)
        {
            if (Request.IsAjaxRequest())
            {
                var products = (List<OrderedProductDTO>)Session["Cart"];
                var product = products?.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                    product.Quantity += 1;
                CartViewModel cvm = new CartViewModel() { Products = products, TotalPrice = products.Sum(p => p.LinePrice) };
                return PartialView("_CartTable", cvm);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //уменьшение количества выбранного товара в корзине
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DecreaseItem(Guid productId)
        {
            if (Request.IsAjaxRequest())
            {
                var products = (List<OrderedProductDTO>)Session["Cart"];
                var product = products?.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                    product.Quantity = (product.Quantity == 1) ? 1 : product.Quantity - 1;
                CartViewModel cvm = new CartViewModel() { Products = products, TotalPrice = products.Sum(p => p.LinePrice) };
                return PartialView("_CartTable", cvm);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //удаление выбранного товара из корзины
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DeleteItem(Guid productId)
        {
            if (Request.IsAjaxRequest())
            {
                var products = (List<OrderedProductDTO>)Session["Cart"];
                var product = products?.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                    products.Remove(product);
                CartViewModel cvm = new CartViewModel() { Products = products, TotalPrice = products.Sum(p => p.LinePrice) };
                return PartialView("_CartTable", cvm);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Оформление нового заказа

        //оформление заказа
        [Authorize]
        public ActionResult MakeOrder()
        {
            //товары в корзине
            List<OrderedProductDTO> products = (List<OrderedProductDTO>)Session["Cart"];
            if (products == null || products.Count == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            double orderPrice = products.Sum(p => p.LinePrice); //общая стоимость заказа
            OrderViewModel ovm = new OrderViewModel()
            {
                OrderPrice = orderPrice
            };
            return View(ovm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MakeOrder(OrderViewModel ovm)
        {
            if (ModelState.IsValid)
            {
                //товары в корзине
                List<OrderedProductDTO> products = (List<OrderedProductDTO>)Session["Cart"];
                if (products == null || products.Count == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                string currentUserId = User.Identity.GetUserId(); //ID текущего пользователя
                //создаём OrderDTO из ViewModel
                OrderDTO orderDTO = new OrderDTO()
                {
                    OrderPrice = products.Sum(p => p.LinePrice),
                    UserId = currentUserId,
                    Products = products,
                    Street = ovm.Street,
                    House = ovm.House,
                    Apartment = ovm.Apartment,
                    PhoneNumber = ovm.PhoneNumber,
                    Note = ovm.Note,
                    Appeal = ovm.Appeal
                };
                //добавляем заказ в БД
                var result = await orderService.AddOrder(orderDTO);
                if (result.Succeeded == true)
                {
                    Session["Cart"] = null;
                    return RedirectToAction("Completed");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(ovm);
                }
            }
            else
                return View(ovm);
        }

        //страница с информацией об успешном оформлении заказа
        [Authorize]
        public ActionResult Completed()
        {
            return View();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            orderService.Dispose();
            base.Dispose(disposing);
        }
    }
}
