using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Data.DTO
{
    public class ClientLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
