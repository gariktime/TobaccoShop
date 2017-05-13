using Microsoft.AspNet.Identity;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {

        }
    }
}
