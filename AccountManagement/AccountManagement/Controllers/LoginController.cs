﻿using AccountManagement.Contracts;
using AccountManagement.Data.DTO;
using AccountManagement.Repository.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Controllers
{
    public class LoginController : Controller
    {

        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config, IClientRepository clientRepository)
        {
            _config = config;
            _clientRepository = clientRepository;
        }




        [HttpPost("Login")]
        public ActionResult<string> Login(ClientLogin input)
        {
            var validation = new ClientLoginValidation(input);

            if (!validation.ValidateFields())
                return BadRequest("Username not valid , check whitespaces");



            var dbClient = _clientRepository.GetExistingClient(input);

            if (validation.ValidateLogin(dbClient))
            {
                var token = validation.GetToken(dbClient, _config);
                return Ok(new { AccessToken = token });
                //return token;
            }
            else
            {
                return Ok(new { Result = "Invalid login,check username or password" });
            }

        }
    }
}
