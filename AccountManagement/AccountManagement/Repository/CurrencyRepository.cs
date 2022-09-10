﻿using System;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace AccountManagement.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;

        public CurrencyRepository(DapperDbContext dataBase, RepositoryContext repositoryContext)
        {
            _dataBase = dataBase;
            _repositoryContext = repositoryContext;
        }


        public bool Create(Currency entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.Code = entity.Code.ToUpper();

            _repositoryContext.Currencies.Add(entity);
            return Save();
        }
        public bool Update(Currency entity)
        {
            entity.DateModified = DateTime.Now;
            entity.Code = entity.Code.ToUpper();
            _repositoryContext.Currencies.Update(entity);
            return Save();
        }

        public bool Delete(Currency entity)
        {
            _repositoryContext.Currencies.Remove(entity);
            return Save();
        }

        public ICollection<Currency> FindAll()
        {
            return _repositoryContext.Currencies.ToList();
        }


        public bool Save() //throw expection here for DUPLICATE UNIQUE data
        {
            var numberRowsAffected = _repositoryContext.SaveChanges();
            return numberRowsAffected > 0;
        }

        public async Task<IEnumerable<CurrencyViewModel>> GetCurrencies()
        {
            using var connection = _dataBase.CreateConnection();
            var currency = await connection.QueryAsync<CurrencyViewModel>("SELECT * FROM Currencies");
            return currency.ToList();
        }


        public int GetCurrencyId(CurrencyViewModel request)
        {
            var currency = _repositoryContext.Currencies.FirstOrDefault(e => e.Code == request.Code);
            _repositoryContext.ChangeTracker.Clear();
            return currency.Id;
        }

        public Currency FindById(int id)
        {
            var currency = _repositoryContext.Currencies.FirstOrDefault(e => e.Id == id);
            _repositoryContext.ChangeTracker.Clear();
            return currency;
        }
    }
}
