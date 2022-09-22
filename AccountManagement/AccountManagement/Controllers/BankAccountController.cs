using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.ErrorHandling;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dapper.SqlMapper;

namespace AccountManagement.Controllers
{
    [Route("api/BankAccounts")]
    [ApiController]
    [Authorize]
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
            if (client==null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Client with id={request.ClientId} does NOT exists");
            
            var currency = _currencyRepository.FindById(request.CurrencyId);
            if (currency == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Currency with id={request.CurrencyId} does NOT exists");

            bankAccount.Currency = currency;
            bankAccount.Client = client;

         
                var succeed = _bankAccountRepository.Create(bankAccount);
              
                if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Bank account created successfully");
           
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error creating the bank account");




        }

        [HttpGet("GetBankAccount/{id}")]
        public async Task<IActionResult> GetBankAccount(int id)
        {

            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound,$"Bank Account with id={id} does NOT exists");

            var bans = await _bankAccountRepository.GetBankAccount(id);
            return Ok(bans);

        }


        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, BankAccountCreateUpdateDto request)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Bank Account with id={id} does NOT exists");

            var isClientValid = _bankAccountRepository.ClientExists(request.ClientId);
            var isCurrencyValid = _bankAccountRepository.CurrencyExists(request.CurrencyId);

            if (!isClientValid) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Client with id={request.ClientId} does NOT exists");
            if (!isCurrencyValid) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Currency with id={request.CurrencyId} does NOT exists");

            bank = _mapper.Map(request, bank);

                var succeed = _bankAccountRepository.Update(bank);

                if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Bank account updated successfully");
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error updating the bank account");

           
        }


        [HttpPut("DeactivateAccount/{id}")]
        public IActionResult DeactivateAccount(int id)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Bank Account with id={id} does NOT exists");

            if (bank.IsActive == false) throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Bank Account with id={id} is already in PASSIVE STATUS");

            var succeed = _bankAccountRepository.DeactivateAccount(bank);

            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Bank account deactivated successfully");
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error deactivating the bank account");
        }


        [HttpPut("ActivateAccount/{id}")]
        public IActionResult ActivateAccount(int id, decimal depositAmount)
        {
            var bank = _bankAccountRepository.FindById(id);
            if (bank == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Bank Account with id={id} does NOT exists");

            if (bank.IsActive == true) throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"Bank Account with id={id} is already in ACTIVE STATUS");

            var succeed = _bankAccountRepository.ActivateAccount(bank, depositAmount);

            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Bank account activated successfully");
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error activating the bank account");

        }


        [HttpGet("GetBankAccounts")]
        public async Task<IActionResult> GetBankAccounts()
        {
            var banks = await _bankAccountRepository.GetBankAccounts();
            return Ok(banks);
        }




    }
}
