using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var (product, productType) = await productService.GetProductParams(id);

            if (productType == ProductType.Hookah)
            {
                var hookah = (HookahDTO)product;
                HookahViewModel hvm = new HookahViewModel() { ProductId = hookah.ProductId, Mark = hookah.Mark, Model = hookah.Model, Country = hookah.Country, Description = hookah.Description, Price = hookah.Price, Height = hookah.Height };
                //return PartialView("_AddHookah", hvm);
                ViewData["PartialViewName"] = "_AddEditHookah";
                ViewData["ProductModel"] = hvm;
                return View();
            }
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEditHookah(HookahViewModel hvm, HttpPostedFileBase uploadImage)
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

            if (ModelState.IsValid)
            {
                //создание DTO из ViewModel
                Mapper.Initialize(cfg => cfg.CreateMap<HookahViewModel, HookahDTO>());
                HookahDTO hookahDto = Mapper.Map<HookahViewModel, HookahDTO>(hvm);
                hookahDto.Image = imagePath;

                if (hvm.ProductId == Guid.Empty) //добавление нового продукта
                {
                    await productService.AddHookah(hookahDto);
                    return RedirectToAction("AddProduct");
                }
                else //изменение существующего
                {
                    await productService.EditHookah(hookahDto);
                    return RedirectToAction("Products");
                }
            }
            ViewData["PartialViewName"] = "_AddEditHookah";
            ViewData["ProductModel"] = hvm;
            return View("Edit");
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

        //удаление товара
        public async Task<ActionResult> Remove(Guid id)
        {
            await productService.RemoveProduct(id);
            return RedirectToAction("Products");
        }

        //список всех продуктов
        public async Task<ActionResult> Products()
        {
            var products = await productService.GetProductsAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<ActionResult> Products(string searchQuery)
        {
            if (Request.IsAjaxRequest())
            {
                List<ProductDTO> products = await productService.GetProductsAsync(p => p.Mark.Contains(searchQuery) ||
                                                                                       p.Model.Contains(searchQuery));
                return PartialView("_ProductList", products);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Работа с заказами

        public async Task<ActionResult> Orders()
        {
            List<OrderDTO> orders = await orderService.GetActiveOrdersAsync();
            Session["OrderStatus"] = "Active";
            return View(orders);
        }

        public async Task<ActionResult> ActiveOrders()
        {
            if (Request.IsAjaxRequest())
            {
                List<OrderDTO> orders = await orderService.GetActiveOrdersAsync();
                Session["OrderStatus"] = "Active";
                return PartialView("_OrderList", orders);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> OnDeliveryOrders()
        {
            if (Request.IsAjaxRequest())
            {
                List<OrderDTO> orders = await orderService.GetOnDeliveryOrdersAsync();
                Session["OrderStatus"] = "OnDelivery";
                return PartialView("_OrderList", orders);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> CompletedOrders()
        {
            if (Request.IsAjaxRequest())
            {
                List<OrderDTO> orders = await orderService.GetCompletedOrdersAsync();
                Session["OrderStatus"] = "Completed";
                return PartialView("_OrderList", orders);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> ChangeStatus(Guid id, string newStatus)
        {
            if (Request.IsAjaxRequest())
            {
                OperationDetails result = null;
                switch (newStatus)
                {
                    case "Active":
                        result = await orderService.MakeOrderActive(id);
                        break;
                    case "OnDelivery":
                        result = await orderService.MakeOrderOnDelivery(id);
                        break;
                    case "Completed":
                        result = await orderService.MakeOrderCompleted(id);
                        break;
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                List<OrderDTO> orders = null;
                switch (Session["OrderStatus"])
                {
                    case "Active":
                        orders = await orderService.GetActiveOrdersAsync();
                        return PartialView("_OrderList", orders);
                    case "OnDelivery":
                        orders = await orderService.GetOnDeliveryOrdersAsync();
                        return PartialView("_OrderList", orders);
                    case "Completed":
                        orders = await orderService.GetCompletedOrdersAsync();
                        return PartialView("_OrderList", orders);
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Работа с пользователями

        public async Task<ActionResult> Users()
        {
            List<UserDTO> users = await userService.GetUsersByRoleAsync("User");
            return View(users);
        }

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

        public async Task<ActionResult> UserDetails(string id)
        {
            UserDTO user = await userService.FindUserByIdAsync(id);
            return View(user);
        }

        public async Task<string> ChangeUserRole(string id, string newRole)
        {
            UserDTO user = await userService.FindUserByIdAsync(id);
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
