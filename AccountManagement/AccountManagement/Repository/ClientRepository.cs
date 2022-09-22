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
using System.Net;
using System.Reflection.Metadata;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.ErrorHandling;
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
            DoesExists(entity, 1);


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
            DoesExists(entity, 2);

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



        private void DoesExists(Client request, int selectSwitch)
        {//selectSwitch : 1 for create , 2 for update 
            if (EmailDoesExists(request, selectSwitch)) throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Email {request.Email} already exists, try another one");
            if (PhoneDoesExists(request, selectSwitch)) throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Phone {request.Phone} already exists, try another one");
            if (UsernameDoesExists(request, selectSwitch)) throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Username '{request.Username}' already exists, try another one");
        }

        /*
        private void DoesExists(Client request, int selectSwitch)
        {//selectSwitch : 1 for create , 2 for update 
            if (EmailDoesExists(request, selectSwitch)) throw new ArgumentException($"Email {request.Email} already exists, try another one");
            if (PhoneDoesExists(request, selectSwitch)) throw new ArgumentException($"Phone {request.Phone} already exists, try another one");
            if (UsernameDoesExists(request, selectSwitch)) throw new ArgumentException($"Username {request.Username} already exists, try another one");
        }
        */


        private bool UsernameDoesExists(Client request, int selectSwitch)
        {
            var client = selectSwitch switch
            {
                1 =>
                    //create new record
                    _repositoryContext.Clients.FirstOrDefault(e => e.Username == request.Username),
                2 =>
                    //update record
                    _repositoryContext.Clients.FirstOrDefault(e =>
                        e.Username == request.Username && e.Id != request.Id),
                _ => null
            };

            return client != null;
        }
        private bool EmailDoesExists(Client request, int selectSwitch)
        {
            var client = selectSwitch switch
            {
                1 =>
                    //create new record
                    _repositoryContext.Clients.FirstOrDefault(e => e.Email == request.Email),
                2 =>
                    //update record
                    _repositoryContext.Clients.FirstOrDefault(e =>
                        e.Email == request.Email && e.Id != request.Id),
                _ => null
            };

            return client != null;
        }
        private bool PhoneDoesExists(Client request, int selectSwitch)
        {
            var client = selectSwitch switch
            {
                1 =>
                    //create new record
                    _repositoryContext.Clients.FirstOrDefault(e => e.Phone == request.Phone),
                2 =>
                    //update record
                    _repositoryContext.Clients.FirstOrDefault(e =>
                        e.Phone == request.Phone && e.Id != request.Id),
                _ => null
            };

            return client != null;
        }
    }


}
