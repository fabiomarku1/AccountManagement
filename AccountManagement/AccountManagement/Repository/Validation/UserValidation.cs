using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Repository.Validation
{
    public class UserValidation
    {
        // private Client _client;
        private readonly ClientRegistrationDto _clientRegistrationDto;
        public UserValidation()
        {
        }
        public UserValidation(ClientRegistrationDto client)
        {
            _clientRegistrationDto = client;

        }


        public bool ValidateThisClient()
        {

            return (Regex.IsMatch(_clientRegistrationDto.FirstName, "^\\S+$") && //no whitespace
                    Regex.IsMatch(_clientRegistrationDto.LastName, "^\\S+$") && //no whitespace
                    Regex.IsMatch(_clientRegistrationDto.Username,
                        @"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$") && // . and _ allowed
                    Regex.IsMatch(_clientRegistrationDto.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") &&
                    Regex.IsMatch(_clientRegistrationDto.Password,
                        @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$") &&
                    Regex.IsMatch(_clientRegistrationDto.Phone, @"^\+?[0-9][0-9]{7,12}$")
                );

        }


        public bool ValidateThisClient(ClientViewModel client)
        {

            return (Regex.IsMatch(client.FirstName, "^\\S+$") && //no whitespace
                    Regex.IsMatch(client.LastName, "^\\S+$") && //no whitespace
                    Regex.IsMatch(client.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") &&
                    Regex.IsMatch(client.Phone, @"^\+?[0-9][0-9]{7,12}$")
                );
        }


        public void HashClient(Client client) //for an update , this is going to be called , edhe pse do therritet kot 
        {
            CreatePassword(_clientRegistrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            client.PasswordHash = passwordHash;
            client.PasswordSalt = passwordSalt;
        }

        /*
         *   public void HashClient(Client client,ClientRepository repository) //for an update , this is going to be called , edhe pse do therritet kot 
        {
            var oldclient = repository.GetExistingClient(client);

            if (oldclient.PasswordHash == client.PasswordHash && oldclient.PasswordSalt == client.PasswordSalt)
            {

            }

                CreatePassword(_clientRegistrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            client.PasswordHash = passwordHash;
            client.PasswordSalt = passwordSalt;
        }
         */


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
