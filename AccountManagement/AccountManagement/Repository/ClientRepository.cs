using AccountManagement.Contracts;
using AccountManagement.Data;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.Repository
{
    public class ClientRepository : IClientRepository
    {

        private readonly RepositoryDbContext _dataBase;
        public ClientRepository(RepositoryDbContext dataBase)
        {
            _dataBase = dataBase;
        }
        public bool Create(Client entity)
        {
            _dataBase.Clients.Add(entity);
            return Save();
        }

        public bool Delete(Client entity)
        {
            _dataBase.Clients.Remove(entity);
            return Save();
        }

        public ICollection<Client> FindAll()
        {
            var clients =_dataBase.Clients.ToList();
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
    }
}
