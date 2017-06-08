using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Services;

namespace TobaccoShop.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void AddBindings()
        {
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<IOrderService>().To<OrderService>();
        }
    }
}
