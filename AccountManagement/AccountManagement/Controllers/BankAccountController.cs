using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
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
            if (client == null) return Ok($"Client with id={request.ClientId} does NOT exists");
            var currency = _currencyRepository.FindById(request.CurrencyId);
            if (currency == null) return Ok($"Currency with id={request.CurrencyId} does NOT exists");

            bankAccount.Currency = currency;
            bankAccount.Client = client;

            try
            {
                var succeed = _bankAccountRepository.Create(bankAccount);
                return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });

            }
            catch (ArgumentException e)
            {
                return Ok(e.Message);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }


        }

        [HttpGet("GetBankAccount/{id}")]
        public async Task<IActionResult> GetBankAccount(int id)
        {

            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound($"Bank Account with id={id} does NOT exists");

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

            if (!isClientValid) return Ok($"Client with id={request.ClientId} does NOT exists");
            if (!isCurrencyValid) return Ok($"Currency with id={request.CurrencyId} does NOT exists");

            bank = _mapper.Map(request, bank);

            try
            {
                var succeed = _bankAccountRepository.Update(bank);
                return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
            }
            catch (ArgumentException e)
            {
                return Ok(e.Message);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }


        [HttpPut("DeactivateAccount/{id}")]
        public IActionResult DeactivateAccount(int id)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");

            if (bank.IsActive == false) return Conflict("This bank account is already in PASSIVE status");

            var succeed = _bankAccountRepository.DeactivateAccount(bank);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpPut("ActivateAccount/{id}")]
        public IActionResult ActivateAccount(int id, decimal depositAmount)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) return NotFound("Bank Account does NOT exists");

            if (bank.IsActive == true) return Conflict("This bank account is already in ACTIVE status");

            var succeed = _bankAccountRepository.ActivateAccount(bank, depositAmount);
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
