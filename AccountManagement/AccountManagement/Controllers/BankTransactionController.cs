using System;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Repository.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
    [Route("api/BankTransaction")]
    [ApiController]
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
                if (bankTransaction.BankAccount == null) return NotFound("Bank account does NOT exists");

                try
                {
                    var succeed = _bankTransactionRepository.Create(bankTransaction);
                    return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });

                }
                catch (ArgumentException e)
                {
                    return Ok(e.Message);
                }

            }


            [HttpGet("GetTransaction/{id}")]
            public IActionResult GetTransaction(int id)
            {
                var transaction = _bankAccountRepository.FindById(id);
                if (transaction == null) return NotFound("Bank Account does NOT exists");

                return Ok(transaction);
            }


            [HttpGet("GetAllTransactions")]
            public async Task<IActionResult> GetBankAccounts()
            {
                var banks = await _bankTransactionRepository.GetTransactions();
                return Ok(banks);
            }

    }

}
