using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Repositories;

namespace TobaccoShop.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connectionString)
        {
            return new UserService(new EFUnitOfWork(connectionString));
        }
    }
}
