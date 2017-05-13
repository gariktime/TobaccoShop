using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Services;
using TobaccoShop.DAL.Repositories;

[assembly: OwinStartup(typeof(TobaccoShop.App_Start.Startup))]

namespace TobaccoShop.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Connect/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return new UserService(new EFUnitOfWork("DbConnection"));
        }
    }
}
