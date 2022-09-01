using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AccountManagement.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;

        }


        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientRepository.GetClients();
            return Ok(clients);
        }


        [HttpGet("{id}", Name = "ClientById")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _clientRepository.GetClientId(id);

            if (client is null)
                return NotFound();
            return Ok(client);
        }



        [HttpPost(nameof(Client))]
        public IActionResult Create(Client entity)
        {
            var client = _clientRepository.Create(entity);
            return Ok(new { Result = true });
        }


    }






}
