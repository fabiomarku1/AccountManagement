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
using AccountManagement.Repository.Validation;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;

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


        public async Task<IEnumerable<ClientViewModel>> GetClients()
        {
            using var connection = _dataBase.CreateConnection();
            var clients = await connection.QueryAsync<ClientViewModel>("SELECT * FROM Clients");
            return clients.ToList();

        }


        public bool Create(Client entity)
        {
            entity.DateCreated = DateTime.Now;

            _repositoryContext.Clients.Add(entity);
            return Save();
        }

        //  data is coming from DTO , so update based on unique keys that are
        public bool Update(Client entity)
        {
            //  var client = GetExistingClient(entity);


            //  if (client != null)
            //   {
            //      entity.Id=client.Id;
            _repositoryContext.Clients.Update(entity);
            //  }
            return Save();
        }


        public bool Delete(Client entity)
        {

                _repositoryContext.Clients.Remove(entity);
                return Save();
            

        }


        public ICollection<Client> FindAll()
        {
            var clients = _repositoryContext.Clients.ToList();
            return clients;
        }


        public bool IsValid(int id) //can be deleted
        {
            var valid = _repositoryContext.Clients.Any(e => e.Id == id);
            return valid;
        }

        public Client GetExistingClient(Client entity)
        {
            //these values are unique for each record at clients
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Email == entity.Email);
            return client;
            //and the user CANNOT CHANGE [Id,Email] and atributet e tjera ndryshohen
            //edhe pse email e shikon , ndersa ID jo , behet nje validim per te mos create a new DTO class
        }
        public Client GetExistingClient(ClientRegistrationDto entity)
        {
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Email == entity.Email);
            return client;
        }


        public Client Login(ClientLogin request, UserValidation validation)
        {
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Username == request.Username);

            if (client == null)
            {
                return null;
            }
            var requestMap = _mapper.Map<Client>(request);

            validation.HashClient(requestMap);


            if (requestMap.PasswordHash == client.PasswordHash && requestMap.PasswordSalt == client.PasswordSalt)
                return client;


            return null;

        }


        public bool Save()
        {
            try
            {
                var changes = _repositoryContext.SaveChanges();
                return changes > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "error while writing changes to db");
                return false;

            }

        }
    }


}
