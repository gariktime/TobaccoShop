using System.Collections.Generic;
using TobaccoShop.DAL.Entities.Products;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.BLL.ListModels
{
    public class HookahListModel
    {
        private IUnitOfWork db;

        public int minPrice { get; private set; }

        public int maxPrice { get; private set; }

        public double minHeight { get; private set; }

        public double maxHeight { get; private set; }

        public IEnumerable<Hookah> Products;

        public List<string> Marks;

        public string[] SelectedMarks { get; set; }

        public HookahListModel(IUnitOfWork uow)
        {
            db = uow;
            minPrice = db.Hookahs.GetPropMinValue(p => p.Price);
            maxPrice = db.Hookahs.GetPropMaxValue(p => p.Price);
            minHeight = db.Hookahs.GetPropMinValue(p => p.Height);
            maxHeight = db.Hookahs.GetPropMaxValue(p => p.Height);
            Marks = db.Hookahs.GetPropValues(p => p.Mark);
            Products = db.Hookahs.GetList();
        }
    }
}
