using AccountManagement.Data;

namespace AccountManagement.Contracts
{
    public interface IBankAccountRepository:IRepositoryBase<BankAccount>
    {

        public bool CurrencyExists(int id);
        public bool ClientExists(int id);
    }
}
