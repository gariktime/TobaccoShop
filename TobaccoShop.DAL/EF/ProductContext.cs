using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities;

namespace TobaccoShop.DAL.EF
{
    public class ProductContext : DbContext
    {
        static ProductContext()
        {
            Database.SetInitializer<ProductContext>(new MyContextInitializer());
        }

        public ProductContext(string connectionString)
            : base(connectionString)
        {

        }

        /// <summary>
        /// Конфигурация базы данных с использованием Fluent API
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderInfoConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Hookah> Hookahs { get; set; }
        public DbSet<HookahTobacco> HookahTobacco { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<OrderInfo> OrderedProductInfo { get; set; }
    }

    #region Model Configuration Fluent API
    class OrderInfoConfiguration : EntityTypeConfiguration<OrderInfo>
    {
        public OrderInfoConfiguration()
        {
            ToTable("OrderInfo");
        }
    }
    #endregion

    class MyContextInitializer : DropCreateDatabaseAlways<ProductContext>
    {
        protected override void Seed(ProductContext db)
        {
            HookahTobacco p1 = new HookahTobacco("Al Fakher", "Apple", 35, 75, 20);
            HookahTobacco p2 = new HookahTobacco("Al Fakher", "Cherry", 70, 75, 20);
            HookahTobacco p3 = new HookahTobacco("Al Fakher", "Mint", 35, 75, 20);
            HookahTobacco p4 = new HookahTobacco("Al Fakher", "Orange", 70, 75, 20);
            Comment com1 = new Comment { Text = "Заебок" };
            Comment com2 = new Comment { Text = "Нормас" };
            p1.Comments.Add(com1);
            p1.Comments.Add(com2);
            db.HookahTobacco.AddRange(new List<HookahTobacco> { p1, p2, p3, p4 });
            db.Comments.AddRange(new List<Comment> { com1, com2 });


            Hookah p5 = new Hookah("KM", "BOER GL Bronze", 45, 9000, 2);
            Hookah p6 = new Hookah("Khalil", "Mamoon Halazone Trimetal", 85, 6800, 1);
            db.Hookahs.AddRange(new List<Hookah> { p5, p6 });

            List<OrderInfo> infos = new List<OrderInfo>();
            OrderInfo info1 = new OrderInfo { Quantity = 3, Product = p1 };
            OrderInfo info2 = new OrderInfo { Quantity = 4, Product = p2 };
            infos.Add(info1);
            infos.Add(info2);
            db.OrderedProductInfo.Add(info1);
            db.OrderedProductInfo.Add(info2);
            Order or1 = new Order { OrderDate = DateTime.Now, Products = infos };
            db.Orders.Add(or1);

            List<OrderInfo> infos2 = new List<OrderInfo>();
            OrderInfo info3 = new OrderInfo { Quantity = 26, Product = p1 };
            infos2.Add(info3);
            Order or2 = new Order { OrderDate = DateTime.Now, Products = infos2 };
            db.Orders.Add(or2);

            db.SaveChanges();
            base.Seed(db);
        }
    }
}
