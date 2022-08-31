using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManagement.Data;

namespace AccountManagement.Contracts
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        public Task<IEnumerable<Client>> GetClients();
        public Task<Client> GetClientId(int id);

        public Task<Client> Create(Client entity);
    }
}
