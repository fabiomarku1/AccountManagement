using System;
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
        private readonly ICheckoutRepository _checkoutRepository;

        private static List<ProductCheckoutDTO> list;
        private static decimal totalSummation;

        public CheckoutController(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository, IBankAccountRepository bankAccountRepository, IBankTransactionRepository bankTransactionRepository, ICheckoutRepository checkoutRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _bankAccountRepository = bankAccountRepository;
            _bankTransactionRepository = bankTransactionRepository;
            _checkoutRepository = checkoutRepository;
        }


        [HttpPut("Chart")]
        public IActionResult Chart(List<int> idOfProducts)
        {
            Dictionary<int, ProductCheckoutDTO> productDictionary = new Dictionary<int, ProductCheckoutDTO>();

            List<ProductCheckoutDTO> listOfProd = new List<ProductCheckoutDTO>();




            int counter = 0;

            foreach (var i in idOfProducts)
            {
                var product = _productRepository.FindById(i);
                if (product == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={i} does NOT exist");


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


            foreach (var i in productDictionary)
            {
                listOfProd.Add(i.Value);
            }

            list=listOfProd;
            totalSummation=totalSum;

            return Ok(totalSum);

        }


        [HttpPut("SelectBankAccount/{bankAccountId}")]
        public IActionResult SelectBank(int bankAccountId)
        {
            var bankAccount = _bankAccountRepository.FindById(bankAccountId);
            if (bankAccount == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                    $"Bank account with id={bankAccount} does not exists");

            //var transaction=_bankTransactionRepository.Create(new BankTransaction()
            //{
            //    Action = ActionCall.Terheqje,
            //    Amount = totalSummation,

            //})

            var sales = new Sales()
            {
                Amount = totalSummation,
                BankAccount = bankAccount,
               // BankAccountId = bankAccount.Id,
                ListOfProducts = list,
                DateCreated = DateTime.Now,
            };


            _checkoutRepository.Create(sales);

            var mappedSale = _mapper.Map<Sales, SalesDTO>(sales);
            mappedSale.ListOfProduct = list;

            return Ok(mappedSale);

            list = null;
            totalSummation=0;

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
