using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dapper.SqlMapper;

namespace AccountManagement.Controllers
{
    [Route("api/BankAccounts")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMapper _mapper;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IClientRepository _clientRepository;

        public BankAccountController(IMapper mapper, IBankAccountRepository bankAccountRepository, IClientRepository clientRepository, ICurrencyRepository currencyRepository)
        {
            _mapper = mapper;
            _bankAccountRepository = bankAccountRepository;
            _clientRepository = clientRepository;
            _currencyRepository = currencyRepository;
        }


        [HttpPost("Create")]
        public IActionResult Create(BankAccountCreateUpdateDto request)
        {
            var bankAccount = _mapper.Map<BankAccount>(request);


            var client = _clientRepository.FindById(request.ClientId);
            if (client == null) return Ok("Client does NOT exists");
            var currency = _currencyRepository.FindById(request.CurrencyId);
            if (currency == null) return Ok("Currency does NOT exists");

            bankAccount.Currency = currency;
            bankAccount.Client = client;


            //var isClientValid = _bankAccountRepository.ClientExists(request.ClientId);
            //if (!isClientValid) return Ok("Client does NOT exists");

            //var isCurrencyValid = _bankAccountRepository.CurrencyExists(request.CurrencyId);
            //if (!isCurrencyValid) return Ok("Currency does NOT exists");

            var succeed = _bankAccountRepository.Create(bankAccount);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("GetBankAccount/{id}")]
        public async Task<IActionResult> GetBankAccount(int id)
        {

            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");
            //
            //   var mappedBank = _mapper.Map<BankAccountGetDto>(bank);

            //  var mapped = _mapper.Map(bank, mappedBank);

            var bans = await _bankAccountRepository.GetBankAccount(id);
            return Ok(bans);

        }


        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, BankAccountCreateUpdateDto request)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");

            var isClientValid = _bankAccountRepository.ClientExists(request.ClientId);
            var isCurrencyValid = _bankAccountRepository.CurrencyExists(request.CurrencyId);

            if (!isClientValid) return Ok("Currency does NOT exists");
            if (!isCurrencyValid) return Ok("Client does NOT exists");

            bank = _mapper.Map(request, bank);

            var succeed = _bankAccountRepository.Update(bank);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });

        }


        //===============================DELETED IT AT THE END , NO NEED================
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");

            var succeed = _bankAccountRepository.Delete(bank);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        } //===============================DELETED IT AT THE END , NO NEED================

        //===============================DELETED IT AT THE END , NO NEED================
        [HttpPut("DeactivateAccount/{id}")]
        public IActionResult DeactivateAccount(int id)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");

            var succeed = _bankAccountRepository.DeactivateAccount(bank);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("GetBankAccounts")]
        public async Task<IActionResult> GetBankAccounts()
        {
            var banks = await _bankAccountRepository.GetBankAccounts();
            return Ok(banks);
        }




    }
}
