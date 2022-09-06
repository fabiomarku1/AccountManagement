using System.Security.Cryptography;
using AccountManagement.Data;
using Microsoft.AspNetCore.Identity;

namespace AccountManagement.Repository.Validation
{
    public class UserValidation
    {
        private Client _client;

        public UserValidation(Client client)
        {
            _client = client;

            CreatePassword(client.Password, out byte[] passwordHash, out byte[] passwordSalt);
            _client.PasswordHash = passwordHash;
            _client.PasswordSalt = passwordSalt;


        }





        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var crypto = new HMACSHA512())
            {
                passwordSalt = crypto.Key;
                passwordHash = crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }


        }

    }
}
