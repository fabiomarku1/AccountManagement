using System;
using System.Net;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.ErrorHandling;
using AccountManagement.Repository.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
    [Route("api/BankTransaction")]
    [ApiController]
    [Authorize]
    public class BankTransactionController : ControllerBase
    {
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IMapper _mapper;



        public BankTransactionController(IMapper mapper, IBankAccountRepository bankAccountRepository, IBankTransactionRepository bankTransactionRepository)
        {
            _mapper = mapper;
            _bankAccountRepository = bankAccountRepository;
            _bankTransactionRepository = bankTransactionRepository;
        }



        [HttpPost("Create")]
        public IActionResult Create(BankTransactionCreateDto request)
        {
            var bankTransaction = _mapper.Map<BankTransaction>(request);

            bankTransaction.BankAccount = _bankAccountRepository.FindById(request.BankAccountId);
            if (bankTransaction.BankAccount == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Bank Account with id={bankTransaction.BankAccountId} does NOT exists");

        
                var succeed = _bankTransactionRepository.Create(bankTransaction);

                if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Bank transaction created successfully");
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error creating the transaction");


        }


        [HttpGet("GetTransaction/{id}")]
        public IActionResult GetTransaction(int id)
        {
            var transaction = _bankTransactionRepository.FindById(id);
            if (transaction == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Transaction with id={id} does NOT exists");
            var mappedTrans = _mapper.Map<BankTransactionGetDto>(transaction);
            return Ok(mappedTrans);
        }


        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetBankAccounts()
        {
            var banks = await _bankTransactionRepository.GetTransactions();
            return Ok(banks);
        }

    }

}
