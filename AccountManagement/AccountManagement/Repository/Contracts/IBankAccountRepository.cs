using AccountManagement.Data;
using AccountManagement.Data.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagement.Contracts
{
    public interface IBankAccountRepository : IRepositoryBase<BankAccount>
    {
        public bool CurrencyExists(int id);
        public bool ClientExists(int id);

        public bool DeactivateAccount(BankAccount entity);
        public bool CodeUserLevelExists(BankAccount entity);
        public Task<IEnumerable<BankAccountGetDto>> GetBankAccounts();
        public Task<BankAccountGetDto> GetBankAccount(int id);

        public List<BankAccount> AccountFromClientId(int id);

    }
}
