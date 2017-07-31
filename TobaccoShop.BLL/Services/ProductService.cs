using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Util;
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

        #region Создание новых продуктов

        public async Task<OperationDetails> AddHookah(HookahDTO hookahDto)
        {
            try
            {
                Hookah hookah = new Hookah()
                {
                    ProductId = Guid.NewGuid(),
                    Mark = hookahDto.Mark.Trim(),
                    Model = hookahDto.Model.Trim(),
                    Description = (hookahDto.Description == null || hookahDto.Description.Trim() == "") ? "Отсутствует" : hookahDto.Description.Trim(),
                    Country = (hookahDto.Country == null || hookahDto.Country.Trim() == "") ? "Нет данных" : hookahDto.Country.Trim(),
                    Height = hookahDto.Height,
                    Price = hookahDto.Price,
                    //если файл не передан, то устанавливаем изображение по умолчанию
                    Image = (hookahDto.Image == null) ? "/Files/ProductImages/defaultImage.jpg" : hookahDto.Image
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
                    Price = htDto.Price
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

        public async Task<OperationDetails> EditHookah(HookahDTO hookahDto)
        {
            try
            {
                Hookah hookah = await db.Products.FindByIdAsync(hookahDto.ProductId) as Hookah;

                hookah.Mark = hookahDto.Mark.Trim();
                hookah.Model = hookahDto.Model.Trim();
                hookah.Description = (hookahDto.Description == null || hookahDto.Description.Trim() == "") ? "Отсутствует" : hookahDto.Description.Trim();
                hookah.Country = (hookahDto.Country == null || hookahDto.Country.Trim() == "") ? "Нет данных" : hookahDto.Country.Trim();
                hookah.Height = hookahDto.Height;
                hookah.Price = hookahDto.Price;
                //если новое изображение не передано, то оставляем старое
                hookah.Image = (hookahDto.Image == null) ? hookah.Image : hookahDto.Image;

                db.Hookahs.Update(hookah);
                await db.SaveAsync();

                return new OperationDetails(true, "Товар успешно обновлен", "");
            }
            catch (DbUpdateConcurrencyException)
            {
                return new OperationDetails(false, "Данный товар ранее уже был изменен", "");
            }
            catch
            {
                return new OperationDetails(false, "При изменении товара произошла ошибка", "");
            }
        }

        #endregion

        //удаление продукта
        public async Task<OperationDetails> RemoveProduct(Guid id)
        {
            try
            {
                db.Products.Delete(id);
                await db.SaveAsync();
                return new OperationDetails(true, "Товар успешно удален", "");
            }
            catch
            {
                return new OperationDetails(false, "При удалении товара произошла ошибка", "");
            }
        }

        #region Методы множеств

        public ProductDTO FindById(Guid id)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<Product, ProductDTO>(db.Products.FindById(id));
        }

        public async Task<ProductDTO> FindByIdAsync(Guid id)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<Product, ProductDTO>(await db.Products.FindByIdAsync(id));
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Product>, List<ProductDTO>>(await db.Products.GetProductsAsync());
        }

        public async Task<List<ProductDTO>> GetProductsAsync(string searchQuery)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<Product>, List<ProductDTO>>(await db.Products.GetProductsAsync(p => p.Mark.Contains(searchQuery) || p.Model.Contains(searchQuery)));
        }

        public async Task<List<HookahDTO>> GetHookahsAsync()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Hookah, HookahDTO>());
            return Mapper.Map<IEnumerable<Hookah>, List<HookahDTO>>(await db.Hookahs.GetProductsAsync());
        }

        public async Task<List<HookahDTO>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks, string[] countries)
        {
            if (marks == null)
            {
                List<string> _marks = await db.Hookahs.GetPropValuesAsync(p => p.Mark);
                marks = _marks.ToArray();
            }
            if (countries == null)
            {
                List<string> _countries = await db.Hookahs.GetPropValuesAsync(p => p.Country);
                countries = _countries.ToArray();
            }

            var hookahs = await db.Hookahs.GetProductsAsync(p => p.Price >= minPrice &&
                                                            p.Price <= maxPrice &&
                                                            p.Height >= minHeight &&
                                                            p.Height <= maxHeight &&
                                                            marks.Contains(p.Mark) &&
                                                            countries.Contains(p.Country));

            Mapper.Initialize(cfg => cfg.CreateMap<Hookah, HookahDTO>());
            return Mapper.Map<IEnumerable<Hookah>, List<HookahDTO>>(hookahs);
        }

        #endregion

        #region Property methods

        public async Task<(int, int, double, double, List<string>, List<string>)> GetHookahProperties()
        {
            int minPrice = await db.Hookahs.GetPropMinValueAsync(p => p.Price);
            int maxPrice = await db.Hookahs.GetPropMaxValueAsync(p => p.Price);
            double minHeight = await db.Hookahs.GetPropMinValueAsync(p => p.Height);
            double maxHeight = await db.Hookahs.GetPropMaxValueAsync(p => p.Height);
            List<string> marks = await db.Hookahs.GetPropValuesAsync(p => p.Mark);
            List<string> countries = await db.Hookahs.GetPropValuesAsync(p => p.Country);

            return (minPrice, maxPrice, minHeight, maxHeight, marks, countries);
        }

        #endregion

        #region Вспомогательные методы #Кудажeбезкостылей

        public async Task<(ProductDTO, ProductType)> GetProductParamsAsync(Guid productId)
        {
            var product = await db.Products.FindByIdAsync(productId);
            if (product is Hookah)
            {
                //Mapper.Initialize(cfg => cfg.CreateMap<Hookah, HookahDTO>());
                Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
                HookahDTO hookah = Mapper.Map<Hookah, HookahDTO>(product as Hookah);
                return (hookah, ProductType.Hookah);
            }
            else
                return (null, ProductType.HookahTobacco);
        }

        #endregion

        #region Добавление комментария к продукту

        public async Task<OperationDetails> AddComment(CommentDTO commentDto)
        {
            try
            {
                //находим товар, к которому добавляем комментарий
                Product product = await db.Products.FindByIdAsync(commentDto.ProductId);
                if (product == null)
                    return new OperationDetails(false, "Товар с указанными Id не найден.", "");
                //создаём Comment из CommentDTO
                Comment comment = new Comment()
                {
                    CommentId = Guid.NewGuid(),
                    CommentDate = DateTime.Now,
                    ProductId = commentDto.ProductId,
                    UserId = commentDto.UserId,
                    Text = commentDto.Text
                };
                //добавляем комментарий в БД
                db.Comments.Add(comment);
                await db.SaveAsync();

                return new OperationDetails(true, "Комментарий успешно добавлен.", "");
            }
            catch
            {
                return new OperationDetails(false, "Ошибка при добавлении комментария.", "");
            }
        }

        #endregion

        public void Dispose()
        {
            db.Dispose();
        }
    }

    public enum ProductType
    {
        [Description("Кальян")]
        Hookah,

        [Description("Табак для кальяна")]
        HookahTobacco
    }
}
