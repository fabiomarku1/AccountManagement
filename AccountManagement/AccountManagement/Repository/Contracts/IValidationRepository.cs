using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Repository.Validation;

namespace AccountManagement.Repository.Contracts
{
    public interface IValidationRepository<T>
    {

        bool ValidateFields();

        void HashClient(Client client);
        void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}

