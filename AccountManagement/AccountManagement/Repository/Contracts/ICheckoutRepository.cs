using AccountManagement.Contracts;
using AccountManagement.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManagement.Data.DTO;

namespace AccountManagement.Repository.Contracts
{
    public interface ICheckoutRepository : IRepositoryBase<Sales>
    {
        public Task<IEnumerable<SalesDTO>> GetTransactions();
    }
}
