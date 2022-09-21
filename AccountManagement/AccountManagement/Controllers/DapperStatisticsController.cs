using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
    [Route("api/Statistics")]
    [ApiController] 
    [Authorize]
    public class DapperStatisticsController : ControllerBase
    {

        private readonly DapperDbContext _dapperDb;
        private readonly IClientRepository _clientRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICategoryRepository _categoryRepository;

        public DapperStatisticsController(IBankAccountRepository bankAccountRepository, IClientRepository clientRepository, DapperDbContext dapperDb, ICategoryRepository categoryRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _clientRepository = clientRepository;
            _dapperDb = dapperDb;
            _categoryRepository = categoryRepository;
        }


        [HttpGet("GetFirstAPI")]
        public IActionResult GetFirstAPI()
        {
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query("select c.Id as ClientId,c.FirstName+' '+c.LastName as Name,b.Code as BankCode,b.Name as BankName,cu.Code as Currency,b.Balance " +
                                        "from Clients c,BankAccounts b , Currencies cu " +
                                        "where c.Id=b.ClientId AND cu.Id=b.CurrencyId");
            return Ok(data);
        }



        [HttpGet("GetTransactionForAccount/{id} AccountId")]
        public IActionResult GetTransactionForAccount(int id)
        {
            var account = _bankAccountRepository.FindById(id);

            if (account == null) return NotFound("Bank account does NOT exists");
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query($"select t.Action,t.Amount,t.DateCreated as Date from BankTransactions t,BankAccounts acc where t.BankAccountId=acc.Id AND t.BankAccountId={id} order by t.DateCreated");
            return Ok(data);
        }


        [HttpGet("GetActiveAccounts/{id} ClientId")]
        public IActionResult GetActiveAccounts(int id)
        {
            var client = _clientRepository.FindById(id);

            if (client == null) return NotFound("Client does NOT exists");
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query($"select b.Code as AccountCode , b.Name as AccountName , c.Code as Currency,b.Balance as CurrentBalance from BankAccounts b,Currencies c where b.CurrencyId=c.Id and b.IsActive='true' and b.ClientId={id}");
            return Ok(data);
        }


        [HttpGet("GetProducts/{id} CategoryId")]
        public IActionResult GetProducts(int id)
        {
            var category = _categoryRepository.FindById(id);

            if (category == null) return NotFound("Client does NOT exists");
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query($"select p.Id as ProductId,p.Name,p.Price,p.ShortDescription from Categories c,Products p where p.CategoryId=c.Id and p.CategoryId={id} order by p.DateCreated");
            return Ok(data);
        }
    }
}
