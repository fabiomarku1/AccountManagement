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
        private readonly ICategoryRepository _categoryRepository;


        public ProductRepository(DapperDbContext dataBase, RepositoryContext repositoryContext, ICategoryRepository categoryRepository)
        {
            _dataBase = dataBase;
            _repositoryContext = repositoryContext;
            _categoryRepository = categoryRepository;
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
            var catepAsync = await connection.QueryAsync<ProductCategoryDto>("select p.Id AS ProductId,p.Name as ProductName,c.Id as CategoryId,c.Code as CategoryCode,C.Description as CategoryDescription from Categories c,Products p where p.CategoryId=c.Id ");
            return catepAsync.ToList();
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

        public int GetProductId(ProductViewModel request)
        {
            throw new NotImplementedException();
        }
    }
}
