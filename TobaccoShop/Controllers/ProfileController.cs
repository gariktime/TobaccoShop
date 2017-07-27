using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Interfaces;

namespace TobaccoShop.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IUserService userService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }
        private IOrderService orderService;

        public ProfileController(IOrderService orService)
        {
            orderService = orService;
        }

        [Authorize]
        public async Task<ActionResult> MyProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            UserDTO currentUser = await userService.FindUserByIdAsync(currentUserId);
            return View(currentUser);
        }

        [Authorize]
        public async Task<ActionResult> Orders()
        {
            string currentUserId = User.Identity.GetUserId();
            List<OrderDTO> orders = await orderService.GetUserOrdersAsync(currentUserId);
            return View(orders);
        }

        protected override void Dispose(bool disposing)
        {
            orderService.Dispose();
            base.Dispose(disposing);
        }
    }
}