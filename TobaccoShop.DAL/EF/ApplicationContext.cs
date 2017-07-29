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
        public DbSet<ShopUser> ShopUsers { get; set; }
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

            db.HookahTobacco.AddRange(new List<HookahTobacco> { p1, p2, p3, p4 });

            Hookah p5 = new Hookah(Guid.NewGuid(), "KM", "BOER GL Bronze", 9000, "Описание", "Германия", 45, "/Files/ProductImages/defaultImage.jpg");
            Hookah p6 = new Hookah(Guid.NewGuid(), "Khalil", "Mamoon Halazone Trimetal", 6800, "Описание", "Азербайджан", 85, "/Files/ProductImages/defaultImage.jpg");
            db.Hookahs.AddRange(new List<Hookah> { p5, p6 });

            db.SaveChanges();
        }
    }
}
