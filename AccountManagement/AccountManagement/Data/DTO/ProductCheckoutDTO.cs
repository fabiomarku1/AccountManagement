using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Data.DTO
{
    public class ProductCheckoutDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;

    }
}
