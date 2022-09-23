using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.Repository.Contracts;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AccountManagement.Repository
{
    public class CheckoutRepository : ICheckoutRepository
    {


        private readonly DapperDbContext _dataBase;
        private readonly RepositoryContext _repositoryContext;


        public CheckoutRepository(RepositoryContext repositoryContext, DapperDbContext dataBase)
        {
            _repositoryContext = repositoryContext;
            _dataBase = dataBase;
        }


        public bool Create(Sales entity)
        {
            _repositoryContext.Sales.Add(entity);
            return Save();
        }

        public async Task<IEnumerable<SalesDTO>> GetTransactions()
        {
            using var connection = _dataBase.CreateConnection();
            var sales = await connection.QueryAsync<SalesDTO>("select * from Sales");
            //  InsertProducts(sales);
            foreach (var i in sales)
            {
                var list = await connection.QueryAsync<ProductCheckoutDTO>($"select * from ProductCheckoutDTO p where SalesId in (select Id from Sales where Id={i.Id})");
                i.ListOfProducts = list.ToList();
            }
            return sales.ToList();
        }


        public Sales FindById(int id)
        {
            var sale = _repositoryContext.Sales.Find(id);
            return sale;
        }

        public bool Update(Sales entity)
        {
            throw new System.NotImplementedException();
        }


        public bool Delete(Sales entity)
        {
            _repositoryContext.Sales.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            var nrOfChanges = _repositoryContext.SaveChanges();
            return nrOfChanges > 0;
        }

        public ICollection<Sales> FindAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
