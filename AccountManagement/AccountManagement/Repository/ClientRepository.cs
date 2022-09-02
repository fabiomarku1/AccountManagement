using AccountManagement.Contracts;
using AccountManagement.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AutoMapper;

namespace AccountManagement.Repository
{
    public class ClientRepository : IClientRepository
    {

        private readonly DapperDbContext _dataBase;
        private readonly IMapper _mapper;
        private readonly RepositoryContext _repositoryContext;


        public ClientRepository(DapperDbContext dataBase, IMapper mapper, RepositoryContext repositoryContext)
        {
            _dataBase = dataBase;
            _mapper = mapper;
            _repositoryContext = repositoryContext;
        }



        public async Task<Client> GetClientId(int id)
        {
            using var connect = _dataBase.CreateConnection();
            var client = await connect.QueryFirstOrDefaultAsync<Client>($"select * from Clients where Id={id} ");
            return client;

        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            using var connection = _dataBase.CreateConnection();
            var clients = await connection.QueryAsync<Client>("SELECT * FROM Clients");
            return clients.ToList();

        }

        public bool Create(Client entity)
        {
            _repositoryContext.Clients.Add(entity);
            return Save();
        }

        //  data is coming from DTO , so update based on unique keys that are
        public bool Update(Client entity)
        {
            _repositoryContext.Clients.Update(entity);
            return Save();
        }


        public bool Delete(Client client)
        {
            _repositoryContext.Clients.Remove(client);
            return Save();
        }


        public ICollection<Client> FindAll()
        {
            var clients = _repositoryContext.Clients.ToList();
            return clients;
        }
        public Client FindById(int id)
        {
            var client = _repositoryContext.Clients.Find(id);
            return client;
        }

        public bool IsValid(int id)
        {
            var valid = _repositoryContext.Clients.Any(e => e.Id == id);
            return valid;
        }
        public bool Save()
        {
            var changes = _repositoryContext.SaveChanges();
            return changes > 0;
        }
    }
}
