using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Contracts
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        public Task<IEnumerable<Client>> GetClients();
        public Task<Client> GetClientId(int id);

        public Task<Client> Create(Client entity);


        public  Task<ActionResult<List<Client>>> CreateNewClient(Client entity);
    }
}
