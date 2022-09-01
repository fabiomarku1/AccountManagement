using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Contracts
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        public Task<IEnumerable<Client>> GetClients();
        public Task<Client> GetClientId(int id);

        //  public bool Create(Client entity);

        public bool CreateNewDto(ClientDto client);
        //public bool CreateNewClient(Client entity);
        public bool Update(Client entity);
    }
}
