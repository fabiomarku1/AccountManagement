using System.Collections.Generic;
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
        private readonly List<string> _errorList = new List<string>();

        public ClientRegisterValidation(ClientRegistrationDto clientRegistration)
        {
            _clientRegistration = clientRegistration;
        }


        public bool ValidateFields()
        {
            if (!Regex.IsMatch(_clientRegistration.FirstName, "^\\S+$"))
                _errorList.Add("FirstName not valid(check whitespaces)");

            if (!Regex.IsMatch(_clientRegistration.LastName, "^\\S+$"))
                _errorList.Add("LastName not valid(check whitespaces)");

            if (!Regex.IsMatch(_clientRegistration.Username,
                   @"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$"))
                _errorList.Add("Username not valid(check whitespaces and special Characters: {. _} ) ");

            if (!Regex.IsMatch(_clientRegistration.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                _errorList.Add("Email not valid");


            if (!Regex.IsMatch(_clientRegistration.Password,
                @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                _errorList.Add("Password not valid. Must contain : A lowercase letter " +
                               ",A uppercase letter , " +
                               "A special character," +
                               "8 characters long ");

            if (!Regex.IsMatch(_clientRegistration.Phone, @"^\+?[0-9][0-9]{7,12}$"))
                _errorList.Add("Phone number not valid (check prefix ex:+355 ," +
                               "Must be between 7-12 digit ");

            return _errorList.Capacity <= 0;

        }//create a dictinary for the error output


        public List<string> GetErrors() => _errorList;

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


        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var crypto = new HMACSHA512(passwordSalt))
            {

                var computedHash = crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
