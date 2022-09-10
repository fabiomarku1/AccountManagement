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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
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

        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }



        [HttpPost("Create")]
        public IActionResult Create(ClientRegistrationDto request)
        {
            var validation = new ClientRegisterValidation(request);


            if (!validation.ValidateFields())
                return BadRequest(validation.GetErrors());

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


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var client = _clientRepository.FindById(id);

            if (client == null)
                return BadRequest($"Client with id={id} does not exist");
            var succeed = _clientRepository.Delete(client);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] ClientRegistrationDto request)
        {
            var existingClient = _clientRepository.FindById(id);
            if (existingClient == null)
                return BadRequest($"Client with id={id} does not exists");


            var validation = new ClientRegisterValidation(request);
            if (!validation.ValidateFields())
                return BadRequest(validation.GetErrors());


            existingClient = _mapper.Map<ClientRegistrationDto, Client>(request, existingClient);
            validation.HashClient(existingClient);
            var succeed = _clientRepository.Update(existingClient);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });

        }

        [HttpGet("PrintDetailed")]
        public IActionResult FindAll()
        {
            return Ok(_clientRepository.FindAll());
        }


    }

}
