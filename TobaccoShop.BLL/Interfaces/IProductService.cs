﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IProductService
    {
        List<Hookah> GetHookahs(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
        Task<List<Hookah>> GetHookahsAsync(int minPrice, int maxPrice, double minHeight, double maxHeight, string[] marks);
    }
}