using AccountManagement.Data;
using AccountManagement.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagement.Contracts
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public Task<IEnumerable<ProductViewModel>> GetProducts();
        public int GetProductId(ProductViewModel request);
    }
}
