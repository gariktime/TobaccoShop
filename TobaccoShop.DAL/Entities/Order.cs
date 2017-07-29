using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TobaccoShop.DAL.Entities.Identity;

namespace TobaccoShop.DAL.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        public double OrderPrice { get; set; }

        [StringLength(75, MinimumLength = 2)]
        public string Appeal { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Street { get; set; }

        [StringLength(25, MinimumLength = 1)]
        public string House { get; set; }

        [StringLength(25, MinimumLength = 1)]
        public string Apartment { get; set; }

        [StringLength(20, MinimumLength = 6)]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public string UserId { get; set; }
        public ShopUser User { get; set; }

        public List<OrderedProduct> Products { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Order()
        {
            Products = new List<OrderedProduct>();
            RowVersion = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        }
    }

    public enum OrderStatus
    {
        [Description("Активный")]
        Active = 0,

        [Description("В доставке")]
        OnDelivery = 1,

        [Description("Выполнен")]
        Completed = 2
    }
}
