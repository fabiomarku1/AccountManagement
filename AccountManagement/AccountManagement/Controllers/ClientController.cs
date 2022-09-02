using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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




        [HttpPost(nameof(Client))]
        public IActionResult Create(ClientDto entity)
        {
            var entityData = _mapper.Map<Client>(entity);
            var succeed = _clientRepository.Create(entityData);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpGet(nameof(DbType))]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientRepository.GetClients();
            return Ok(clients);
        }


        [HttpDelete]
        public IActionResult Delete(ClientDto client)
        {
            var entityData = _mapper.Map<Client>(client);
            var succeed = _clientRepository.Delete(entityData);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpGet("{id}", Name = "ClientById")]
        public IActionResult GetById(int id)
        {
            var client = _clientRepository.FindById(id);
            if (client is null)
                return NotFound();
            return Ok(client);
        }

        [HttpPut(nameof(Client))]
        public IActionResult Update(Client client)
        {
            //     var entity = _mapper.Map<Client>(client);
            var succeed = _clientRepository.Update(client);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }






    }





}
