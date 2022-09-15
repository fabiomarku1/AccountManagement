using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.Repository.Contracts;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using static Dapper.SqlMapper;

namespace AccountManagement.Repository
{
    public class BankTransactionRepository:IBankTransactionRepository
    {

        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;

        private readonly IBankAccountRepository _bankAccountRepository;

        public BankTransactionRepository(DapperDbContext dataBase, RepositoryContext repositoryContext, IBankAccountRepository bankAccountRepository)
        {
            _dataBase = dataBase;
            _repositoryContext = repositoryContext;
            _bankAccountRepository = bankAccountRepository;
        }


        public bool Create(BankTransaction entity)
        {
            DoAction(entity);

            entity.BankAccount.DateModified = entity.DateCreated=DateTime.Now;

            _repositoryContext.BankTransactions.Add(entity);
            return Save();

        }

        public BankTransaction FindById(int id)
        {
            var transaction=_repositoryContext.BankTransactions.Find(id);
            return transaction;
        }

        public bool Delete(BankTransaction entity)
        {
            _repositoryContext.BankTransactions.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            try
            {
                var changes = _repositoryContext.SaveChanges();
                return changes > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              throw  new Exception("Error while saving records at Database");

            }
        }

        public async Task<IEnumerable<BankTransactionGetDto>> GetTransactions()
        {
            using var connection = _dataBase.CreateConnection();
            var banks = await connection.QueryAsync<BankTransactionGetDto>("select * from BankTransactions");
            return banks.ToList();
        }

        public ICollection<BankTransaction> FindAll()
        {
            throw new NotImplementedException();
        }
        public bool Update(BankTransaction entity)
        {
            throw new NotImplementedException();
        }

        private void DoAction(BankTransaction request)
        {
            
            if (request.Action == ActionCall.Depositim) 
                Deposit(request);
            else if (request.Action == ActionCall.Terheqje)
            {
                if (request.BankAccount.IsActive == false) throw new ArgumentException("The given bank account is not active.\nPlease ACTIVATE it in order to withdrawal");
                Withdrawal(request);
            }
            else throw new ArgumentException(" Not valid ACTION , 1-Depostim / 2-Terheqje");

        }


        private void Deposit(BankTransaction request)
        {
            request.BankAccount.Balance += request.Amount;
          //  request.IsActive = true;

        }

        private void Withdrawal(BankTransaction request)
        {
            var account=request.BankAccount;

            if (account.Balance > request.Amount)
                account.Balance -= request.Amount;
            else
                throw new ArgumentException("Not enough BALANCE");

        }

    }
}
