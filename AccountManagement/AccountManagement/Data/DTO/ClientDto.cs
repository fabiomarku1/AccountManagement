using System.ComponentModel.DataAnnotations;
using System;
using System.Text.RegularExpressions;

namespace AccountManagement.Data.DTO
{
    public class ClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        // public DateTime DateCreated { get; set; } = DateTime.Now;//?????????????

        // public byte[] PasswordHash { get; set; } 
        // public byte[] PasswordSalt { get; set; }
    }
}
