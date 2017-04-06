namespace TobaccoShop.Models
{
    public class OrederInfo
    {
        public int ID { get; set; }
        public int Quantity { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}
