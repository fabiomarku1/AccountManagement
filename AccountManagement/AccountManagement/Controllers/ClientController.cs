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
        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }



        [HttpPost("Create")]
        public IActionResult Create(ClientRegistrationDto entity)
        {
            //CALL VALIDATION METHOD()  for username password
            //do also validation for existing of UNIQUE ATTRIBUTES


            var entityData = _mapper.Map<Client>(entity);

            UserValidation validation = new UserValidation(entityData);


            var succeed = _clientRepository.Create(entityData);

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


        [HttpPut("Update")]
        public IActionResult Update(ClientViewModel entity)
        {
            var client = _mapper.Map<Client>(entity);
            var succeed = _clientRepository.Update(client);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("PrintDetailed")]
        public IActionResult FindAll()
        {
            return Ok(_clientRepository.FindAll());
        }







    }





}
