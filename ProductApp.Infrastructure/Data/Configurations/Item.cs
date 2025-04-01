namespace ProductApp.Infrastructure.Data.Configurations
{
    public class Item
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
 
        // Navigation property for the related Product
        public Product Product { get; set; }
    }
}