using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Repository.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AccountManagement.Repository.Validation
{
    public class ClientLoginValidation : IValidationRepository<ClientLogin>
    {
        private readonly ClientLogin _clientLogin;
        public ClientLoginValidation(ClientLogin clientLogin)
        {
            _clientLogin = clientLogin;
        }

        public bool ValidateLogin(Client client)
        {
            //   var client = repositoryContext.Clients.FirstOrDefault(e => e.Username == _clientLogin.Username);

            if (client == null)
            {
                return false;
            }

            return (VerifyPassword(_clientLogin.Password, client.PasswordHash, client.PasswordSalt) &&
                    _clientLogin.Username.Equals(client.Username));

        }

        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var crypto = new HMACSHA512(passwordSalt))
            {

                var computedHash = crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }


        public void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var crypto = new HMACSHA512())
            {
                passwordSalt = crypto.Key;
                passwordHash = crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void HashClient(Client client)
        {
            CreatePassword(_clientLogin.Password, out byte[] passwordHash, out byte[] passwordSalt);
            client.PasswordHash = passwordHash;
            client.PasswordSalt = passwordSalt;
        }

        public bool ValidateFields()
        {
            return (Regex.IsMatch(_clientLogin.Username,
                @"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$"));

        }


        public string GetToken(Client client, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid,client.Id.ToString()),
                //   new Claim(ClaimTypes.Authentication,client.Id.ToString()),
                new Claim(ClaimTypes.Email,client.Email),
                new Claim(ClaimTypes.Name,client.Username),
              //  new Claim(ClaimTypes.Role,"Admin")

            };


            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value
            ));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var createJwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(createJwtSecurityToken);

            return token;

        }

    }
}
