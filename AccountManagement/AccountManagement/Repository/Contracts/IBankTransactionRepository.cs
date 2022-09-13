using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;

namespace AccountManagement.Repository.Contracts
{
    public interface IBankTransactionRepository : IRepositoryBase<BankTransaction>
    {
        public Task<IEnumerable<BankTransactionGetDto>> GetTransactions();
    }
}