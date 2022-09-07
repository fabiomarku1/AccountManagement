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

        public ClientController(IClientRepository clientRepository, IMapper mapper, IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _config = configuration;
        }



        [HttpPost("Create")]
        public IActionResult Create(ClientRegistrationDto request)
        {
            var validation = new ClientRegisterValidation(request);

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
        public IActionResult Delete(ClientViewModel request)
        {
            var client = _mapper.Map<Client>(request);

            var entityData = _clientRepository.GetExistingClient(client);

            // _mapper.Map(entityData, client);

            var succeed = _clientRepository.Delete(entityData);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        //================================================================================================
        [HttpPut("Update")]
        public IActionResult Update(ClientRegistrationDto entity)
        {
            var validation = new ClientRegisterValidation(entity);

            if (!validation.ValidateThisClient())
                return BadRequest("Input not correct");



            var client = _mapper.Map<Client>(entity);

            var entityData = _clientRepository.GetExistingClient(entity);

            var mapped = _mapper.Map(client, entityData);

            if (!validation.CheckForChanges(mapped, entity))
                validation.HashClient(mapped);

            //if (!validation.CheckForChanges(_clientRepository.GetExistingClient(client), entity))
            //    validation.HashClient(client);

            var succeed = _clientRepository.Update(mapped);
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
            var validation = new UserValidation();

            var client = _clientRepository.Login(input, validation);

            if (client == null)
                return BadRequest("login failed");

            var token = validation.GetToken(client, _config);

            //var isSuccesful = _clientRepository.Login(input);

            return token;

        }


    }

}
