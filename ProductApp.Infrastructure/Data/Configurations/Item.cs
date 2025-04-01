using System.ComponentModel.DataAnnotations;

namespace ProductApp.Infrastructure.Data.Configurations
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
 
        // Navigation property for the related Product
        public required Product Product { get; set; }
    }
}