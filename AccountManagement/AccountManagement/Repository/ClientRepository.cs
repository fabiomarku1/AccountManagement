﻿using AccountManagement.Contracts;
using AccountManagement.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using Dapper;

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
            var query = "select * from Client where Id=@id ";

            using (var connect = _dataBase.CreateConnection())
            {
                var client = await connect.QueryFirstOrDefaultAsync<Client>(query);
                return client;
            }
            {

            }
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            var query = "SELECT * FROM Client";
            using (var connection = _dataBase.CreateConnection())
            {
                var clients = await connection.QueryAsync<Client>(query);
                return clients.ToList();
            }

        }
        public async Task<Client> Create(Client entity)
        {
            var query =
                "insert into Client (Id,FirstName,Last,Email,Birthday,Phone,DateCreated,DateModified,Username,PasswordHash,PasswordSalt) values (@Id,@FirstName,@Last,@Email,@Birthday,@Phone,@DateCreated,@DateModified,@Username,@PasswordHash,@PasswordSalt)" +
                "select cast(scope_identity() as int";

            var parameters = new DynamicParameters();

            parameters.Add("Id", entity.Id, DbType.Int32);
            parameters.Add("FirstName", entity.FirstName, DbType.AnsiString);
            parameters.Add("LastName", entity.LastName, DbType.AnsiString);
            parameters.Add("Email", entity.Email, DbType.AnsiString);
            parameters.Add("Birthday", entity.Birthday, DbType.DateTime);
            parameters.Add("Phone", entity.Phone, DbType.AnsiString);
            parameters.Add("DateCreated", entity.DateCreated, DbType.DateTime);
            parameters.Add("DateModified", entity.DateModified, DbType.DateTime);
            parameters.Add("Username", entity.Username, DbType.AnsiString);
            parameters.Add("PasswordHash", entity.PasswordHash, DbType.Byte);
            parameters.Add("PasswordSalt", entity.PasswordSalt, DbType.Byte);

            using (var connect = _dataBase.CreateConnection())
            {
                var id = await connect.QuerySingleAsync<int>(query, parameters);

                var createClient = new Client
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Birthday = entity.Birthday,
                    Phone = entity.Phone,
                    DateCreated = entity.DateCreated,
                    DateModified = entity.DateModified,
                    Username = entity.Username,
                    PasswordHash = entity.PasswordHash,
                    PasswordSalt = entity.PasswordSalt
                };
                return createClient;
            }




        }



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

                public bool Update(Client entity)
                {
                    _dataBase.Clients.Update(entity);
                    return Save();
                }
                */
    }
}
