using System;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Data.DTO;
using Dapper;

namespace AccountManagement.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;


        public ProductRepository(DapperDbContext dataBase, RepositoryContext repositoryContext)
        {
            _dataBase = dataBase;
            _repositoryContext = repositoryContext;
        }


        public bool Create(Product entity)
        {
            entity.DateCreated = DateTime.Now;
            _repositoryContext.Products.Add(entity);
            return Save();
        }
        public bool Update(Product entity)
        {
            entity.DateModified = DateTime.Now;

            _repositoryContext.Products.Update(entity);
            return Save();
        }

        public bool Delete(Product entity)
        {
            _repositoryContext.Products.Remove(entity);
            return Save();
        }


        public ICollection<Product> FindAll()
        {
            return _repositoryContext.Products.ToList();
        }

        public async Task<IEnumerable<ProductGDto>> GetProducts()
        {
            using var connection = _dataBase.CreateConnection();
            var product = await connection.QueryAsync<ProductGDto>("select * from Products");
            return product.ToList();
        }


        public async Task<IEnumerable<ProductCategoryDto>> GetProductsAndCategories()
        {
            using var connection = _dataBase.CreateConnection();
            var categories = await connection.QueryAsync<ProductCategoryDto>("select p.Id AS ProductId,p.Name as ProductName,c.Id as CategoryId,c.Code as CategoryCode,C.Description as CategoryDescription from Categories c,Products p where p.CategoryId=c.Id ");
            return categories.ToList();
        }


        public bool Save()
        {
            var nrOfRowsAffected = _repositoryContext.SaveChanges();
            return nrOfRowsAffected > 0;
        }

        public Product FindById(int id)
        {
            var product = _repositoryContext.Products.Find(id);
            return product;
        }

    }
}
