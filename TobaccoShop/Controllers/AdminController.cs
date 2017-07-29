using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Services;
using TobaccoShop.Models.ProductModels;

namespace TobaccoShop.Controllers
{
    public class AdminController : Controller
    {
        private IUserService userService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }

        private IProductService productService;
        private IOrderService orderService;

        public AdminController(IProductService prService, IOrderService oService)
        {
            productService = prService;
            orderService = oService;
        }

        #region Добавление/редактирование/удаление/список/поиск товаров

        //добавление нового товара
        [Authorize(Roles = "Moderator, Admin")]
        public ActionResult AddProduct()
        {
            return View();
        }

        //редактирование товара
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> EditProduct(Guid? productId)
        {
            if (productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var (product, productType) = await productService.GetProductParamsAsync((Guid)productId);

            if (productType == ProductType.Hookah)
            {
                HookahDTO hookah = (HookahDTO)product;
                HookahViewModel hvm = new HookahViewModel() { ProductId = hookah.ProductId, Mark = hookah.Mark, Model = hookah.Model, Country = hookah.Country, Description = hookah.Description, Price = hookah.Price, Height = hookah.Height, Image = hookah.Image };
                return View("AddEditHookah", hvm);
            }
            return RedirectToAction("Products");
        }

        //Добавление нового / редактирование существующего Hookah
        [Authorize(Roles = "Moderator, Admin")]
        public ActionResult AddEditHookah()
        {
            HookahViewModel hvm = new HookahViewModel();
            return View(hvm);
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEditHookah(HookahViewModel hvm, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                //добавление изображения к товару
                string imagePath = null;
                if (uploadImage != null)
                {
                    if (!uploadImage.ContentType.StartsWith("image"))
                        ModelState.AddModelError("", "Недопустимый тип файла");
                    else
                    {
                        imagePath = "/Files/ProductImages/" + Path.GetFileName(uploadImage.FileName);
                        uploadImage.SaveAs(Server.MapPath("~" + imagePath));
                    }
                }

                //создание DTO из ViewModel
                Mapper.Initialize(cfg => cfg.CreateMap<HookahViewModel, HookahDTO>());
                HookahDTO hookahDto = Mapper.Map<HookahViewModel, HookahDTO>(hvm);
                hookahDto.Image = imagePath;

                if (hvm.ProductId == Guid.Empty) //добавление нового продукта
                {
                    await productService.AddHookah(hookahDto);
                    return RedirectToAction("AddEditHookah");
                }
                else //изменение существующего
                {
                    await productService.EditHookah(hookahDto);
                    return RedirectToAction("Products");
                }
            }
            else
                return View("AddEditHookah", hvm);
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //удаление товара
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> Remove(Guid productId)
        {
            await productService.RemoveProduct(productId);
            return RedirectToAction("Products");
        }

        //список всех продуктов
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> Products()
        {
            var products = await productService.GetProductsAsync();
            return View(products);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> Products(string searchQuery)
        {
            if (Request.IsAjaxRequest())
            {
                List<ProductDTO> products = await productService.GetProductsAsync(searchQuery);
                return PartialView("_ProductList", products);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Работа с заказами

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> OrderDetails(Guid? orderId)
        {
            if (orderId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
            {
                OrderDTO order = await orderService.FindByIdAsync((Guid)orderId);
                if (order != null)
                    return View(order);
                else
                    return Content("Заказ с указанным ID не найден.");
            }
        }

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> Orders()
        {
            List<OrderDTO> orders = await orderService.GetActiveOrdersAsync();
            //Session["OrderStatus"] = "Active";
            return View(orders);
        }

        //поиск заказа по номеру
        [Authorize(Roles = "Moderator, Admin")]
        [HttpPost]
        public async Task<ActionResult> FindOrder(int? orderNumber)
        {
            if (Request.IsAjaxRequest())
            {
                if (orderNumber != null)
                {
                    OrderDTO order = await orderService.FindByNumberAsync((int)orderNumber);
                    if (order != null)
                    {
                        List<OrderDTO> orders = new List<OrderDTO>() { order };
                        return PartialView("_OrderList", orders);
                    }
                    else
                        return PartialView("_OrderList", null);
                }
                else
                {
                    return PartialView("_OrderList", null);
                }
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> ActiveOrders()
        {
            if (Request.IsAjaxRequest())
            {
                List<OrderDTO> orders = await orderService.GetActiveOrdersAsync();
                //Session["OrderStatus"] = "Active";
                return PartialView("_OrderList", orders);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> OnDeliveryOrders()
        {
            if (Request.IsAjaxRequest())
            {
                List<OrderDTO> orders = await orderService.GetOnDeliveryOrdersAsync();
                //Session["OrderStatus"] = "OnDelivery";
                return PartialView("_OrderList", orders);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> CompletedOrders()
        {
            if (Request.IsAjaxRequest())
            {
                List<OrderDTO> orders = await orderService.GetCompletedOrdersAsync();
                //Session["OrderStatus"] = "Completed";
                return PartialView("_OrderList", orders);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPost]
        public async Task<ActionResult> ChangeOrderStatus(Guid orderId, string newStatus)
        {
            OperationDetails result = await orderService.ChangeOrderStatus(orderId, newStatus);

            if (result.Succeeded == true)
                return RedirectToAction("OrderDetails", new { orderId = orderId });
            else
                return Content(result.Message);

            //List<OrderDTO> orders = null;
            //switch (Session["OrderStatus"])
            //{
            //    case "Active":
            //        orders = await orderService.GetActiveOrdersAsync();
            //        return PartialView("_OrderList", orders);
            //    case "OnDelivery":
            //        orders = await orderService.GetOnDeliveryOrdersAsync();
            //        return PartialView("_OrderList", orders);
            //    case "Completed":
            //        orders = await orderService.GetCompletedOrdersAsync();
            //        return PartialView("_OrderList", orders);
            //    default:
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

        }

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<ActionResult> DeleteOrder(Guid orderId)
        {
            OperationDetails result = await orderService.DeleteOrder(orderId);
            if (result.Succeeded == true)
                return RedirectToAction("Orders");
            else
                return Content(result.Message);
        }

        #endregion

        #region Работа с пользователями

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Users()
        {
            List<UserDTO> users = await userService.GetUsersByRoleAsync("User");
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UsersByRole(string role)
        {
            if (Request.IsAjaxRequest())
            {
                List<UserDTO> users = null;
                switch (role)
                {
                    case "User":
                        users = await userService.GetUsersByRoleAsync("User");
                        return PartialView("_UserList", users);
                    case "Moderator":
                        users = await userService.GetUsersByRoleAsync("Moderator");
                        return PartialView("_UserList", users);
                    case "Admin":
                        users = await userService.GetUsersByRoleAsync("Admin");
                        return PartialView("_UserList", users);
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UserSearch(string userName)
        {
            if (Request.IsAjaxRequest())
            {
                List<UserDTO> users = await userService.GetUsersByNameAsync(userName);
                return PartialView("_UserList", users);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UserDetails(string userId)
        {
            UserDTO user = await userService.FindUserByIdAsync(userId);
            if (user != null)
                return View(user);
            else
                return Content("Пользователь с указанным ID не найден.");
        }

        [Authorize(Roles = "Admin")]
        public async Task<string> ChangeUserRole(string id, string newRole)
        {
            string currUserId = User.Identity.GetUserId();
            UserDTO user = await userService.FindUserByIdAsync(id);
            if (id == currUserId)
                return user.Role + " Невозможно изменить роль своей учетной записи.";
            var result = await userService.ChangeUserRole(id, user.Role, newRole);
            if (result.Succeeded == true)
            {
                return newRole;
                //user.Role = newRole;
                //return View("UserDetails", user);
            }
            else
                return "";
        }

        #endregion

        #region Статистика

        [Authorize(Roles = "Admin")]
        public ActionResult Statistics()
        {
            return View();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            productService.Dispose();
            base.Dispose(disposing);
        }
    }
}
