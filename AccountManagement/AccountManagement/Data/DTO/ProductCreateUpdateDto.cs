using Microsoft.AspNetCore.Http;

namespace AccountManagement.Data.DTO
{
    public class ProductCreateUpdateDto
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

    }
}
