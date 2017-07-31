using System.Collections.Generic;

namespace TobaccoShop.Models
{
    public class StatisticsViewModel
    {
        public int[] OrderStatusStatistics { get; set; }

        public List<int> OrderCountStatistics { get; set; }

        public List<(double, double, double)> OrderPriceStatistics { get; set; }

        public List<(double, double)> OrderProductsStatistics { get; set; }
    }
}
