using AccountManagement.Data;
using AccountManagement.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagement.Contracts
{
    public interface ICurrencyRepository : IRepositoryBase<Currency>
    {
        public Task<IEnumerable<CurrencyViewModel>> GetCurrencies();

    }
}
