using System.ComponentModel.DataAnnotations;

namespace ProductApp.Infrastructure.Data.Configurations
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
 