using System;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Data
{
    public class Client
    {
       [Key] public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } //unique
        public DateTime Birthday { get; set; }
        public string Phone { get; set; } //unique
        public DateTime DateCreated { get; set; }= DateTime.Now;
        public DateTime DateModified { get; set; }


        public string Username { get; set; } //unique
        public byte[] PasswordHash { get; set; } //need to be hashed
        public byte[] PasswordSalt { get; set; }
    }
}
