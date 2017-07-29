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

        public string Appeal { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Apartment { get; set; }

        public string PhoneNumber { get; set; }

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
