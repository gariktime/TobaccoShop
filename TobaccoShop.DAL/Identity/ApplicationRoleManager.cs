using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store)
            : base(store)
        {

        }
    }
}
