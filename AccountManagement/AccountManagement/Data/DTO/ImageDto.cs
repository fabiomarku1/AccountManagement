using Microsoft.AspNetCore.Http;

namespace AccountManagement.Data.DTO
{
    public class ImageDto
    {
        public IFormFile Image { get; set; }

        public string ImageName;
    }
}
