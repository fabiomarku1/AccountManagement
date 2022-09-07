using System.Linq;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Repository.Contracts;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

namespace AccountManagement.Repository.Validation
{
    public class ClientRegisterValidation : IValidationRepository<ClientRegistrationDto>
    {

        private readonly ClientRegistrationDto _clientRegistration;

        public ClientRegisterValidation(ClientRegistrationDto clientRegistration)
        {
            _clientRegistration = clientRegistration;
            // CheckForChanges();
        }


        public bool ValidateThisClient()
        {
            return (Regex.IsMatch(_clientRegistration.FirstName, "^\\S+$") && //no whitespace
                    Regex.IsMatch(_clientRegistration.LastName, "^\\S+$") && //no whitespace
                    Regex.IsMatch(_clientRegistration.Username,
                        @"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$") && // . and _ allowed
                    Regex.IsMatch(_clientRegistration.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") &&
                    Regex.IsMatch(_clientRegistration.Password,
                        @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$") &&
                    Regex.IsMatch(_clientRegistration.Phone, @"^\+?[0-9][0-9]{7,12}$")
                );

        }


        public bool HashClient()
        {
            throw new System.NotImplementedException();
        }

        public void HashClient(Client client) //for an update , this is going to be called , edhe pse do therritet kot 
        {
            CreatePassword(_clientRegistration.Password, out byte[] passwordHash, out byte[] passwordSalt);
            client.PasswordHash = passwordHash;
            client.PasswordSalt = passwordSalt;
        }

        public void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var crypto = new HMACSHA512())
            {
                passwordSalt = crypto.Key;
                passwordHash = crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        public bool CheckForChanges(Client request, ClientRegistrationDto loginRequest)
        {
            return VerifyPassword(loginRequest.Password, request.PasswordHash, request.PasswordSalt);

        }


        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var crypto = new HMACSHA512(passwordSalt))
            {

                var computedHash = crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
