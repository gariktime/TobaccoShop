using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
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

        public Product FindById(Guid id)
        {
            return db.Products.FindById(id);
        }

        public async Task<Product> FindByIdAsync(Guid id)
        {
            return await db.Products.FindByIdAsync(id);
        }

        public async Task<OperationDetails> RemoveProduct(Guid id)
        {
            try
            {
                Product product = await db.Products.FindByIdAsync(id);
                db.Products.Delete(product);
                await db.SaveAsync();
                return new OperationDetails(true, "Товар успешно удален", "");
            }
            catch
            {
                return new OperationDetails(false, "При удалении товара произошла ошибка", "");
            }
        }

        #region Создание новых продуктов

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

        #endregion

        #region Редактирование продуктов

        public async Task<OperationDetails> EditHookah(Guid id, HookahDTO hookahDto)
        {
            try
            {
                Hookah hookah = await db.Hookahs.FindByIdAsync(id);

                hookah.Mark = hookahDto.Mark;
                hookah.Model = hookahDto.Model;
                hookah.Country = hookahDto.Country;
                hookah.Description = hookahDto.Description;
                hookah.Height = hookahDto.Height;
                hookah.Price = hookahDto.Price;
                hookah.Available = hookahDto.Available;
                hookah.Image = (hookahDto.Image == null) ? hookah.Image : hookahDto.Image;

                db.Hookahs.Update(hookah);
                await db.SaveAsync();

                return new OperationDetails(true, "Товар успешно обновлен", "");
            }
            catch(DbUpdateConcurrencyException)
            {
                return new OperationDetails(false, "Данный товар ранее уже был изменен", "");
            }
            catch
            {
                return new OperationDetails(false, "При изменении товара произошла ошибка", "");
            }
        }

        #endregion

        #region Функции множеств

        public List<Product> GetProducts()
        {
            return db.Products.GetAll();
        }

        public List<Product> GetProducts(Func<Product, bool> predicate)
        {
            return db.Products.GetAll(predicate);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await db.Products.GetAllAsync();
        }

        public async Task<List<Product>> GetProductsAsync(Expression<Func<Product, bool>> predicate)
        {
            return await db.Products.GetAllAsync(predicate);
        }

        public List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks)
        {
            if (marks == null)
                return db.Hookahs.GetAll(p => p.Price >= minPrice &&
                                               p.Price <= maxPrice &&
                                               p.Height >= minHeight &&
                                               p.Height <= maxHeight);
            else
                return db.Hookahs.GetAll(p => p.Price >= minPrice &&
                                               p.Price <= maxPrice &&
                                               p.Height >= minHeight &&
                                               p.Height <= maxHeight &&
                                               marks.Contains(p.Mark));
        }

        public async Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks)
        {
            if (marks == null)
                return await db.Hookahs.GetAllAsync(p => p.Price >= minPrice &&
                                                          p.Price <= maxPrice &&
                                                          p.Height >= minHeight &&
                                                          p.Height <= maxHeight);
            else
                return await db.Hookahs.GetAllAsync(p => p.Price >= minPrice &&
                                                          p.Price <= maxPrice &&
                                                          p.Height >= minHeight &&
                                                          p.Height <= maxHeight &&
                                                          marks.Contains(p.Mark));
        }

        #endregion

        #region Вспомогательные методы #Кудажeбезкостылей

        public ProductType GetProductType(Product product)
        {
            if (product is Hookah)
                return ProductType.Hookah;
            else
                return ProductType.HookahTobacco;
        }

        public Hookah ProductAsHookah(Product product)
        {
            return product as Hookah;
        }

        public HookahTobacco ProductAsHookahTobacco(Product product)
        {
            return product as HookahTobacco;
        }

        #endregion
    }

    public enum ProductType
    {
        [Description("Кальян")]
        Hookah,

        [Description("Табак для кальяна")]
        HookahTobacco
    }
}
