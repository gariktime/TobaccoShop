namespace TobaccoShop.DAL.Entities
{
    public class OrderInfo
    {
        public int ID { get; set; }
        public int Quantity { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public OrderInfo()
        {

        }
    }
}
