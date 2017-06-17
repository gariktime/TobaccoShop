using AutoMapper;
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
                HookahViewModel hvm = new HookahViewModel() { ProductId = hookah.ProductId, Mark = hookah.Mark, Model = hookah.Model, Country = hookah.Country, Description = hookah.Description, Price = hookah.Price, Height = hookah.Height, Image = hookah.Image };
                //return PartialView("_AddHookah", hvm);
                ViewData["PartialViewName"] = "_AddHookah";
                ViewData["Model"] = hvm;
                return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddHookah([Bind(Exclude = "ProductId, Image")] HookahViewModel hvm, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                //если добавлено изображение к продукту
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                }

                //создание DTO из ViewModel
                Mapper.Initialize(cfg => cfg.CreateMap<HookahViewModel, HookahDTO>());
                HookahDTO hookahDto = Mapper.Map<HookahViewModel, HookahDTO>(hvm);

                if (hvm.ProductId == Guid.Empty) //добавление нового продукта
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
            return PartialView("_AddHookah", hvm);
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

        #region Работа с заказами

        public async Task<ActionResult> Orders()
        {
            List<OrderDTO> orders = await orderService.GetActiveOrdersAsync();
            Session["OrderStatus"] = "Active";
            return View(orders);
        }

        public async Task<ActionResult> ActiveOrders()
        {
            List<OrderDTO> orders = await orderService.GetActiveOrdersAsync();
            Session["OrderStatus"] = "Active";
            return PartialView("_OrderList", orders);
        }

        public async Task<ActionResult> OnDeliveryOrders()
        {
            List<OrderDTO> orders = await orderService.GetOnDeliveryOrdersAsync();
            Session["OrderStatus"] = "OnDelivery";
            return PartialView("_OrderList", orders);
        }

        public async Task<ActionResult> CompletedOrders()
        {
            List<OrderDTO> orders = await orderService.GetCompletedOrdersAsync();
            Session["OrderStatus"] = "Completed";
            return PartialView("_OrderList", orders);
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

    }
}
