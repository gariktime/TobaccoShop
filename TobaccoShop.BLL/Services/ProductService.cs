using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.BLL.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork db;

        public ProductService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> AddHookah(HookahDTO hookahDto)
        {
            try
            {
                Hookah hookah = new Hookah()
                {
                    Mark = hookahDto.Mark,
                    Model = hookahDto.Model,
                    Description = hookahDto.Description,
                    Country = hookahDto.Country,
                    Height = hookahDto.Height,
                    Price = hookahDto.Price,
                    Available = hookahDto.Available
                };
                db.Hookahs.Add(hookah);
                await db.SaveAsync();
                return new OperationDetails(true, "Товар успешно добавлен", "");
            }
            catch
            {
                return new OperationDetails(false, "При добавлении товара произошла ошибка", "");
            }
        }

        public async Task<OperationDetails> AddHookahTobacco(HookahTobaccoDTO htDto)
        {
            try
            {
                HookahTobacco tobacco = new HookahTobacco()
                {
                    Mark = htDto.Mark,
                    Model = htDto.Model,
                    Description = htDto.Description,
                    Country = htDto.Country,
                    Weight = htDto.Weight,
                    Price = htDto.Price,
                    Available = htDto.Available
                };
                //db.HookahTobacco.Add(tobacco);
                await db.SaveAsync();
                return new OperationDetails(true, "Товар успешно добавлен", "");
            }
            catch
            {
                return new OperationDetails(false, "При добавлении товара произошла ошибка", "");
            }
        }

        public List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks)
        {
            if (marks == null)
                return db.Hookahs.GetList(p => p.Price >= minPrice &&
                                               p.Price <= maxPrice &&
                                               p.Height >= minHeight &&
                                               p.Height <= maxHeight);
            else
                return db.Hookahs.GetList(p => p.Price >= minPrice &&
                                               p.Price <= maxPrice &&
                                               p.Height >= minHeight &&
                                               p.Height <= maxHeight &&
                                               marks.Contains(p.Mark));
        }

        public async Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks)
        {
            if (marks == null)
                return await db.Hookahs.GetListAsync(p => p.Price >= minPrice &&
                                                          p.Price <= maxPrice &&
                                                          p.Height >= minHeight &&
                                                          p.Height <= maxHeight);
            else
                return await db.Hookahs.GetListAsync(p => p.Price >= minPrice &&
                                                          p.Price <= maxPrice &&
                                                          p.Height >= minHeight &&
                                                          p.Height <= maxHeight &&
                                                          marks.Contains(p.Mark));
        }
    }
}
