using System.Net;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using AccountManagement.ErrorHandling;
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


        [HttpGet("GetClientAccountData")]
        public IActionResult GetFirstAPI()
        {
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query("select c.Id as ClientId,c.FirstName+' '+c.LastName as Name,b.Code as BankCode,b.Name as BankName,cu.Code as Currency,b.Balance " +
                                        "from Clients c,BankAccounts b , Currencies cu " +
                                        "where c.Id=b.ClientId AND cu.Id=b.CurrencyId");
            return Ok(data);
        }



        [HttpGet("GetTransactionForAccount/{accountId}")]
        public IActionResult GetTransactionForAccount(int accountId)
        {
            var account = _bankAccountRepository.FindById(accountId);

            if (account == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Bank account with id={accountId} does NOT exists");
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query($"select t.Action,t.Amount,t.DateCreated as Date from BankTransactions t,BankAccounts acc where t.BankAccountId=acc.Id AND t.BankAccountId={accountId} order by t.DateCreated");
            return Ok(data);
        }


        [HttpGet("GetActiveAccounts/{clientId}")]
        public IActionResult GetActiveAccounts(int clientId)
        {
            var client = _clientRepository.FindById(clientId);

            if (client == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Client with id={clientId} does NOT exists");
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query($"select b.Code as AccountCode , b.Name as AccountName , c.Code as Currency,b.Balance as CurrentBalance from BankAccounts b,Currencies c where b.CurrencyId=c.Id and b.IsActive='true' and b.ClientId={clientId}");
            return Ok(data);
        }


        [HttpGet("GetProducts/{categoryId}")]
        public IActionResult GetProducts(int categoryId)
        {
            var category = _categoryRepository.FindById(categoryId);

            if (category == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Category with id={categoryId} does NOT exists");
            var connection = _dapperDb.CreateConnection();

            var data = connection.Query($"select p.Id as ProductId,p.Name,p.Price,p.ShortDescription from Categories c,Products p where p.CategoryId=c.Id and p.CategoryId={categoryId} order by p.DateCreated");
            return Ok(data);
        }
    }
}
