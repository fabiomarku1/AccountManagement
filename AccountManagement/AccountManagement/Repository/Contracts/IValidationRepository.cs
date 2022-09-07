using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Repository.Validation;

namespace AccountManagement.Repository.Contracts
{
    public interface IValidationRepository<T>
    {

        bool ValidateThisClient();

        bool HashClient();

        bool CheckForChanges(Client client, ClientRegistrationDto requestDto);

        void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt);



    }
}
