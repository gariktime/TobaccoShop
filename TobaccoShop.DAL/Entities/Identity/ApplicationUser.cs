using Microsoft.AspNet.Identity.EntityFramework;

namespace TobaccoShop.DAL.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
