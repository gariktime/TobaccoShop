using Microsoft.AspNet.Identity;
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

        #region Работа с заказами

        public ActionResult MakeOrder()
        {
            List<OrderedProductDTO> products = (List<OrderedProductDTO>)Session["Cart"];
            if (products == null || products.Count == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string currUserId = User.Identity.GetUserId();
            if (currUserId == null)
                return RedirectToAction("Login", "Connect");
            double orderPrice = products.Sum(p => p.LinePrice);
            OrderViewModel ovm = new OrderViewModel()
            {
                OrderPrice = orderPrice
            };
            return View(ovm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmOrder(OrderViewModel ovm)
        {
            List<OrderedProductDTO> products = (List<OrderedProductDTO>)Session["Cart"];
            if (products == null || products.Count == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string currUserId = User.Identity.GetUserId();
            if (currUserId == null)
                return RedirectToAction("Login", "Connect");
            UserDTO currentUser = await UserService.GetCurrentUser(currUserId);
            OrderDTO orderDTO = new OrderDTO()
            {
                OrderPrice = ovm.OrderPrice,
                User = currentUser,
                Products = products,
                OrderDate = DateTime.Now,
                Street = ovm.Street,
                House = ovm.House,
                Apartment = ovm.Apartment,
                PhoneNumber = ovm.Apartment,
                Note = ovm.Note
            };
            var result = await orderService.AddOrder(orderDTO);
            return RedirectToAction("Index", "Home");
        }

        #endregion

    }
}
