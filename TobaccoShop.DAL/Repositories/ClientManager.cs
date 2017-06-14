using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Database { get; set; }

        public ClientManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
        }

        public ClientProfile FindById(string id)
        {
            return Database.ClientProfiles.Include(p => p.Orders).FirstOrDefault(p => p.Id == id);
        }

        public async Task<ClientProfile> FindByIdAsync(string id)
        {
            return await Database.ClientProfiles.Include(p => p.Orders).FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
