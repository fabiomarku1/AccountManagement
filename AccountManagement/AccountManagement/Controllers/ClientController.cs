using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.Repository.Validation;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AccountManagement.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        
        public ClientController(IClientRepository clientRepository, IMapper mapper,IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _config = configuration;
        }



        [HttpPost("Create")]
        public IActionResult Create(ClientRegistrationDto request)
        {
            //CALL VALIDATION METHOD()  for username password
            //do also validation for existing of UNIQUE ATTRIBUTES

            var validation = new UserValidation(request);
            if (!validation.ValidateThisClient())
                return BadRequest("Input not correct");
            //if validation wrong =>  exit/return error 
            // no need to go to repository

            var entity = _mapper.Map<Client>(request);

            validation.HashClient(entity);

            var succeed = _clientRepository.Create(entity);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientRepository.GetClients();
            return Ok(clients);
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(ClientViewModel client)
        {
            var entityData = _mapper.Map<Client>(client);
            var succeed = _clientRepository.Delete(entityData);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        //================================================================================================
        [HttpPut("Update")]
        public IActionResult Update(ClientRegistrationDto entity)
        {
            var validation = new UserValidation(entity);

            if (!validation.ValidateThisClient())
                return BadRequest("Input not correct");



            var client = _mapper.Map<Client>(entity);
            validation.HashClient(client);
            var succeed = _clientRepository.Update(client);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }
        //==================================================================================================

        [HttpGet("PrintDetailed")]
        public IActionResult FindAll()
        {
            return Ok(_clientRepository.FindAll());
        }



        [HttpPost("Login")]
        public ActionResult<string> Login(ClientLogin input)
        {
            var client=_clientRepository.Login(input);

            var validation = new UserValidation();

            string token = validation.GetToken(client);

            //var isSuccesful = _clientRepository.Login(input);
            if (client==null)
                return BadRequest("incorrect");
            return token;

        }


    }

}
