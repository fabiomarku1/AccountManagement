using System;
using System.Collections.Generic;
using System.Linq;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
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
            entity.Client = _clientRepository.FindById(entity.ClientId);
            entity.Currency = _currencyRepository.FindById(entity.CurrencyId);

            entity.DateCreated=DateTime.Now;
            _repositoryContext.BankAccounts.Add(entity);
            return Save();

        }


        public BankAccount FindById(int id)
        {
            return _repositoryContext.BankAccounts.Find(id);
        }

        public bool Update(BankAccount entity)
        {
            entity.DateModified = DateTime.Now;
            _repositoryContext.BankAccounts.Update(entity);
            return Save();
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


    }
}
