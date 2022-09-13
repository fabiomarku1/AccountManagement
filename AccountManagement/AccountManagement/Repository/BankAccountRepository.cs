using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AccountManagement.Repository
{
    public class BankAccountRepository : IBankAccountRepository
    {

        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IClientRepository _clientRepository;


        public BankAccountRepository(DapperDbContext dataBase, RepositoryContext repositoryContext, ICurrencyRepository currencyRepository, IClientRepository clientRepository)
        {
            _dataBase = dataBase;
            _repositoryContext = repositoryContext;
            _currencyRepository = currencyRepository;
            _clientRepository = clientRepository;
        }


        public bool Create(BankAccount entity)
        {
            if (CodeUserLevelExists(entity)) return false; //do a list for errors here;

            //entity.Client = _clientRepository.FindById(entity.ClientId);
            //entity.Currency = _currencyRepository.FindById(entity.CurrencyId);

            entity.DateCreated = DateTime.Now;
            _repositoryContext.BankAccounts.Add(entity);
            return Save();

        }


        public BankAccount FindById(int id)
        {
            return _repositoryContext.BankAccounts.Find(id);
        }


        public bool Update(BankAccount entity)
        {
            if (CodeUserLevelExists(entity)) return false; //do a list for errors here;

            entity.DateModified = DateTime.Now;
            _repositoryContext.BankAccounts.Update(entity);
            return Save();
        }

        public bool DeactivateAccount(BankAccount entity)
        {
            entity.DateModified = DateTime.Now;
            entity.IsActive = false;

            _repositoryContext.BankAccounts.Update(entity);
            return Save();

        }

        public bool CodeUserLevelExists(BankAccount newAccount)
        {
            var code = newAccount.Code;

            var listOfAccounts = AccountFromClientId(newAccount.ClientId);

            foreach (var i in listOfAccounts)
            {
                if (i.Code.Equals(code)) return true;
            }
            return false;
        }

        public bool Delete(BankAccount entity)
        {
            _repositoryContext.BankAccounts.Remove(entity);
            return Save();
        }

        public ICollection<BankAccount> FindAll()
        {
            var banks = _repositoryContext.BankAccounts.ToList();
            return banks;
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




        public bool CurrencyExists(int id)
        {
            var isValid = _currencyRepository.FindById(id);
            return isValid != null;
        }

        public bool ClientExists(int id)
        {
            var isValid = _clientRepository.FindById(id);
            return isValid != null;
        }

        public async Task<IEnumerable<BankAccountGetDto>> GetBankAccounts()
        {
            using var connection = _dataBase.CreateConnection();
            var banks = await connection.QueryAsync<BankAccountGetDto>("select * from BankAccounts");
            return banks.ToList();
        }


        public List<BankAccount> AccountFromClientId(int id)
        {
            using var connection = _dataBase.CreateConnection();
            var banks = connection.Query<BankAccount>($"select * from BankAccounts where ClientId={id}").ToList();
            return banks;
        }


        public async Task<BankAccountGetDto> GetBankAccount(int id)
        {
            using var connection = _dataBase.CreateConnection();
            var banks = await connection.QueryAsync<BankAccountGetDto>($"select * from BankAccounts where Id={id}");
            return banks.SingleOrDefault();
        }
    }
}
