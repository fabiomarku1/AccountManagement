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
using AccountManagement.Data.Model;

namespace AccountManagement.Repository
{
    public class ClientRepository : IClientRepository
    {

        private readonly DapperDbContext _dataBase;
        public ClientRepository(DapperDbContext dataBase)
        {
            _dataBase = dataBase;
        }
        public async Task<Client> GetClientId(int id)
        {
            using var connect = _dataBase.CreateConnection();
            var client = await connect.QueryFirstOrDefaultAsync<Client>($"select * from Client where Id={id} ");
            return client;

        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            using var connection = _dataBase.CreateConnection();
            var clients = await connection.QueryAsync<Client>("SELECT * FROM Client");
            return clients.ToList();

        }

        public bool Create(Client entity)
        {
            var connect = _dataBase.CreateConnection();
            string query =
                "insert into Client(FirstName,LastName,Email,Birthday,Phone,DateCreated,DateModified,Username,PasswordHash,PasswordSalt) values (@FirstName,@LastName,@Email,@Birthday,@Phone,@DateCreated,@DateModified,@Username,@PasswordHash,@PasswordSalt)";

            var rowsAffected = connect.Execute(query, new
            {
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Birthday,
                entity.Phone,
                entity.DateCreated,
                entity.DateModified,
                entity.Username,
                entity.PasswordHash,
                entity.PasswordSalt,

            });
            // var client = connect.ExecuteAsync("insert into Client(FirstName,LastName,Email,Birthday,Phone,DateCreated,DateModified,Username,PasswordHash,PasswordSalt) values (@FirstName,@LastName,@Email,@Birthday,@Phone,@DateCreated,@DateModified,@Username,@PasswordHash,@PasswordSalt)", entity);

            return rowsAffected > 0 ? true : false;
        }



        public bool CreateNewClient(Client entity)
        {
            var connect = _dataBase.CreateConnection();
            string query =
                "insert into Client(FirstName,LastName,Email,Birthday,Phone,DateCreated,DateModified,Username,PasswordHash,PasswordSalt) values (@FirstName,@LastName,@Email,@Birthday,@Phone,@DateCreated,@DateModified,@Username,@PasswordHash,@PasswordSalt)";

            var rowsAffected = connect.Execute(query, new
            {
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Birthday,
                entity.Phone,
                entity.DateCreated,
                entity.DateModified,
                entity.Username,
                entity.PasswordHash,
                entity.PasswordSalt,

            });
            // var client = connect.ExecuteAsync("insert into Client(FirstName,LastName,Email,Birthday,Phone,DateCreated,DateModified,Username,PasswordHash,PasswordSalt) values (@FirstName,@LastName,@Email,@Birthday,@Phone,@DateCreated,@DateModified,@Username,@PasswordHash,@PasswordSalt)", entity);

            return rowsAffected > 0 ? true : false;

        }

        public bool Update(Client entity)
        {
            throw new NotImplementedException();
        }


        //public bool Update(Client entity)
        //{
        //    var connection = _dataBase.CreateConnection();


        //}



        /*
                public bool Delete(Client entity)
                {
                    _dataBase.Clients.Remove(entity);
                    return Save();
                }

                public ICollection<Client> FindAll()
                {
                    var clients = _dataBase.Clients.ToList();
                    return clients;
                }

                public Client FindById(int id)
                {
                    var client = _dataBase.Clients.Find(id);
                    return client;
                }





                public bool IsValid(int id)
                {
                    var valid = _dataBase.Clients.Any(e => e.Id == id);
                    return valid;
                }

                public bool Save()
                {
                    var changes = _dataBase.SaveChanges();
                    return changes > 0;
                }

            
                */
    }
}
