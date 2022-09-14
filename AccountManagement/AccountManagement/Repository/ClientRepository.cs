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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Repository
{
    public class ClientRepository : IClientRepository
    {

        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;


        public ClientRepository(DapperDbContext dataBase, RepositoryContext repositoryContext)
        {
            _dataBase = dataBase;
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
            DoesExists(entity);


            entity.DateCreated = DateTime.Now;

            _repositoryContext.Clients.Add(entity);
            return Save();
        }

        public Client FindById(int id)
        {
            var client = _repositoryContext.Clients.Find(id);
            //  _repositoryContext.ChangeTracker.Clear();
            return client;
        }

        public bool Update(Client entity)
        {
            entity.DateModified = DateTime.Now;
            _repositoryContext.Clients.Update(entity);
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

        public Client GetExistingClient(ClientLogin entity)
        {
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Username == entity.Username);
            return client;
        }


        public bool Save()
        {
            var changes = _repositoryContext.SaveChanges(); 
            return changes > 0;
        }



        private void DoesExists(Client request)
        {
            if (EmailDoesExists(request)) throw new ArgumentException("This EMAIL exists, try another one");
            if (PhoneDoesExists(request)) throw new ArgumentException("This PHONE NUMBER exists, try another one");
            if (UsernameDoesExists(request)) throw new ArgumentException("This USERNAME exists, try another one");
        }

        private bool UsernameDoesExists(Client request)
        {
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Username == request.Username);
            return client!=null;
        }
        private bool EmailDoesExists(Client request)
        {
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Email == request.Email );
            return client != null;
        }
        private bool PhoneDoesExists(Client request)
        {
            var client = _repositoryContext.Clients.FirstOrDefault(e => e.Phone==request.Phone);
            return client != null;
        }
    }


}
