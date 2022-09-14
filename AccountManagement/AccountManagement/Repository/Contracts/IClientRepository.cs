using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.Repository.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Contracts
{
    public interface IClientRepository : IRepositoryBase<Client>
    {
        public Task<IEnumerable<ClientViewModel>> GetClients();

        public Client GetExistingClient(ClientLogin entity);

    }
}
