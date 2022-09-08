using System;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<ProductViewModel>> GetProducts()
        {
            using var connection = _dataBase.CreateConnection();
            var product = await connection.QueryAsync<ProductViewModel>("select * from Products");
            return product.ToList();
        }

        public int GetProductId(ProductViewModel request)
        {
            var product = _repositoryContext.Products.FirstOrDefault(e => e.CategoryId == request.CategoryId);
            _repositoryContext.ChangeTracker.Clear();
            if (product != null)
                return product.Id;
            return -1;
        }

        
        //public async Task<IEnumerable<CategoryViewModel>> GetCategoryAtProducts()
        //{
        //    using var connection = _dataBase.CreateConnection();
        //    var catepAsync = await connection.QueryAsync<CategoryViewModel>("select * from Categories c,Product p where p.CategoryId=c.Id ");
        //    return catepAsync.ToList();
        //}


        public bool Save()
        {
            var nrOfRowsAffected = _repositoryContext.SaveChanges();
            return nrOfRowsAffected > 0;
        }


    }
}
