namespace TobaccoShop.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connectionString);
    }
}
