using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
    [Route("api/BankAccounts")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMapper _mapper;

        public BankAccountController(IMapper mapper, IBankAccountRepository bankAccountRepository)
        {
            _mapper = mapper;
            _bankAccountRepository = bankAccountRepository;
        }


        [HttpPost("Create")]
        public IActionResult Create(BankAccountCreateUpdateDto request)
        {
            
            var bankAccount = _mapper.Map<BankAccount>(request);

            //var isClientValid = _bankAccountRepository.ClientExists(request.ClientId);
            //if (!isClientValid) return Ok("Client does NOT exists");

            //var isCurrencyValid = _bankAccountRepository.CurrencyExists(request.CurrencyId);
            //if (!isCurrencyValid) return Ok("Currency does NOT exists");

            var succeed = _bankAccountRepository.Create(bankAccount);

            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("GetBankAccount/{id}")]
        public IActionResult GetBankAccount(int id)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");
            return Ok(bank);
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

            var succeed=_bankAccountRepository.Update(bank);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });

        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var bank= _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");
           
            var succeed= _bankAccountRepository.Delete(bank);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }




    }
}
