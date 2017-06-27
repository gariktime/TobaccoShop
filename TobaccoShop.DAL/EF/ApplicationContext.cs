using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Entities.Products;

namespace TobaccoShop.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new MyContextInitializer());
        }

        public ApplicationContext(string connectionString)
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
        public DbSet<OrderedProduct> OrderedProductInfo { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
    }

    #region Model Configuration Fluent API
    class OrderInfoConfiguration : EntityTypeConfiguration<OrderedProduct>
    {
        public OrderInfoConfiguration()
        {
            ToTable("OrderInfo");
        }
    }
    #endregion

    class MyContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            HookahTobacco p1 = new HookahTobacco(Guid.NewGuid(), "Al Fakher", "Apple", 70, "Табак1", "Египет", 45, "/Files/ProductImages/defaultImage.jpg");
            HookahTobacco p2 = new HookahTobacco(Guid.NewGuid(), "Al Fakher", "Cherry", 45, "Табак2", "Пакистан", 45, "/Files/ProductImages/defaultImage.jpg");
            HookahTobacco p3 = new HookahTobacco(Guid.NewGuid(), "Al Fakher", "Mint", 100, "Табак3", "Камыши", 45, "/Files/ProductImages/defaultImage.jpg");
            HookahTobacco p4 = new HookahTobacco(Guid.NewGuid(), "Al Fakher", "Orange", 100, "Табак4", "Египет", 45, "/Files/ProductImages/defaultImage.jpg");
            Comment com1 = new Comment { Text = "Заебок" };
            Comment com2 = new Comment { Text = "Нормас" };
            p1.Comments.Add(com1);
            p1.Comments.Add(com2);
            db.HookahTobacco.AddRange(new List<HookahTobacco> { p1, p2, p3, p4 });
            db.Comments.AddRange(new List<Comment> { com1, com2 });


            Hookah p5 = new Hookah(Guid.NewGuid(), "KM", "BOER GL Bronze", 9000, "Описание", "Германия", 45, "/Files/ProductImages/defaultImage.jpg");
            Hookah p6 = new Hookah(Guid.NewGuid(), "Khalil", "Mamoon Halazone Trimetal", 6800, "Описание", "Азербайджан", 85, "/Files/ProductImages/defaultImage.jpg");
            db.Hookahs.AddRange(new List<Hookah> { p5, p6 });

            //List<OrderedProduct> infos = new List<OrderedProduct>();
            //OrderedProduct info1 = new OrderedProduct { Id = Guid.NewGuid(), Quantity = 3, Product = p1 };
            //OrderedProduct info2 = new OrderedProduct { Id = Guid.NewGuid(), Quantity = 4, Product = p2 };
            //infos.Add(info1);
            //infos.Add(info2);
            //db.OrderedProductInfo.Add(info1);
            //db.OrderedProductInfo.Add(info2);
            //Order or1 = new Order { OrderId = Guid.NewGuid(), OrderDate = DateTime.Now, Products = infos, Status = OrderStatus.Completed };
            //db.Orders.Add(or1);

            //List<OrderedProduct> infos2 = new List<OrderedProduct>();
            //OrderedProduct info3 = new OrderedProduct { Quantity = 26, Product = p1 };
            //infos2.Add(info3);
            //Order or2 = new Order { OrderId = Guid.NewGuid(), OrderDate = DateTime.Now, Products = infos2, Status = OrderStatus.OnDelivery };
            //db.Orders.Add(or2);

            db.SaveChanges();
        }
    }
}
