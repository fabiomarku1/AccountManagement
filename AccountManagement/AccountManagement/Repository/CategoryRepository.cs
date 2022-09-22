using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccountManagement.ErrorHandling;

namespace AccountManagement.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;

        public CategoryRepository(DapperDbContext dataBase, RepositoryContext repositoryContext)
        {
            _dataBase = dataBase;
            _repositoryContext = repositoryContext;
        }


        public bool Create(Category entity)
        {
            if (DoesExist(entity)) throw new HttpStatusCodeException(HttpStatusCode.Conflict,$"Category with CODE={entity.Code} already exists");

            entity.DateCreated = DateTime.Now;
            entity.Code = entity.Code.ToUpper();
            _repositoryContext.Categories.Add(entity);
            return Save();
        }
        public Category FindById(int id)
        {
            var category = _repositoryContext.Categories.Find(id);
            return category;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            using var connection = _dataBase.CreateConnection();
            var currency = await connection.QueryAsync<CategoryViewModel>("SELECT * FROM Categories");
            return currency.ToList();
        }

        public bool Update(Category entity)
        {
            if (DoesExistUpdate(entity)) throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Category with CODE={entity.Code} already exists");

            entity.DateModified = DateTime.Now;
            entity.Code = entity.Code.ToUpper();
            _repositoryContext.Categories.Update(entity);
            return Save();
        }

        public bool Delete(Category entity)
        {
            _repositoryContext.Categories.Remove(entity);
            return Save();
        }

        public ICollection<Category> FindAll()
        {
            return _repositoryContext.Categories.ToList();
        }

        public bool Save() //change return value to an error
        {
            try
            {
                var nrOfRowsAffected = _repositoryContext.SaveChanges();
                return nrOfRowsAffected > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "error while writing changes to db");
                return false;

            }

        }

        private bool DoesExist(Category request)
        {
            var category = _repositoryContext.Categories.FirstOrDefault(e => e.Code == request.Code);
            return category != null;
        }

        private bool DoesExistUpdate(Category request)
        {
            var category = _repositoryContext.Categories.FirstOrDefault(e => e.Code == request.Code);

            return category != null && (request.Id != category.Id);
        }
    }
}
