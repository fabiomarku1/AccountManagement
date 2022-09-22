using AccountManagement.Contracts;
using AccountManagement.Data.DTO;
using AccountManagement.Data;
using AccountManagement.ErrorHandling;
using AccountManagement.Repository;
using AccountManagement.Repository.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AccountManagement.Controllers
{
    [Route("api/Checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly CheckoutRepository _checkoutRepository;

        public CheckoutController(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository, IBankAccountRepository bankAccountRepository, CheckoutRepository checkoutRepository, IBankTransactionRepository bankTransactionRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _bankAccountRepository = bankAccountRepository;
            _checkoutRepository = checkoutRepository;
            _bankTransactionRepository = bankTransactionRepository;
        }


        [HttpPut("CheckoutProducts")]
        public IActionResult Checkout(int BankAccountId, List<int> idOfProducts)
        {
            Dictionary<int, ProductCheckoutDTO> productDictionary = new Dictionary<int, ProductCheckoutDTO>();


            foreach (var i in idOfProducts)
            {
                var product = _productRepository.FindById(i);
                if (product == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={i} does NOT exist");
                var mapped = _mapper.Map<Product, ProductCheckoutDTO>(product);

                if (!productDictionary.ContainsKey(i))
                {
                    productDictionary.Add(i, mapped);
                }
                else
                {
                    productDictionary[i].Quantity++;
                }

            }

            decimal totalSum = productDictionary.Sum(i => i.Value.Quantity * i.Value.Price);

            var bankAccount = _bankAccountRepository.FindById(BankAccountId);

            if (bankAccount.Balance < totalSum)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Not sufficient balance");

            return DoCheckout(totalSum, bankAccount);

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private IActionResult DoCheckout(decimal sum, BankAccount bankAccount)
        {

            var trans = new BankTransaction()
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                Action = ActionCall.Terheqje,
                Amount = sum,

            };



            var result = _bankTransactionRepository.Create(trans);

            return Ok(result);

        }

    }
}
