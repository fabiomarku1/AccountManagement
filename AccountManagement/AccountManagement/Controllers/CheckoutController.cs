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
using System.Threading.Tasks;

namespace AccountManagement.Controllers
{
    [Route("api/Checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly ICheckoutRepository _checkoutRepository;


        private static readonly Dictionary<int, ProductCheckoutDTO> ProductDictionary =
            new Dictionary<int, ProductCheckoutDTO>();

        private static List<ProductCheckoutDTO> ListOfProductsDto = new List<ProductCheckoutDTO>();
        private static decimal _totalSummation;

        public CheckoutController(IProductRepository productRepository, IMapper mapper,
            IBankAccountRepository bankAccountRepository, IBankTransactionRepository bankTransactionRepository,
            ICheckoutRepository checkoutRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _bankAccountRepository = bankAccountRepository;
            _bankTransactionRepository = bankTransactionRepository;
            _checkoutRepository = checkoutRepository;
        }

        [HttpPut("AddItemToChart/{id}")]
        public IActionResult AddItemToChart(int id)
        {
            var product = _productRepository.FindById(id);
            if (product == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                    $"Error trying to fetch the product with id={id}");

            var productMapped = _mapper.Map<Product, ProductCheckoutDTO>(product);

            if (!ProductDictionary.ContainsKey(id))
            {
                ProductDictionary.Add(id, productMapped);
                throw new HttpStatusCodeException(HttpStatusCode.OK,
                    $"Product with id={id} added successfully to the chart");
            }
            else
            {
                ProductDictionary.Remove(id);
                throw new HttpStatusCodeException(HttpStatusCode.OK, $"Product with id={id} removed from the chart ");
            }

        }

        [HttpPut("UpdateQuantityForProduct/{id}")]
        public IActionResult UpdateQuantity(int id, int quantityRequest)
        {
            if (!ProductDictionary.ContainsKey(id))
                throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                    $"Product with id={id} does NOT exists in the chart");

            if (quantityRequest == 0) //just increment quantity by one
            {
                ProductDictionary[id].Quantity++;
            }
            else ProductDictionary[id].Quantity = quantityRequest;

            throw new HttpStatusCodeException(HttpStatusCode.OK, "Product quantity updated successfully");
        }



        //List<int> idOfProducts
        [HttpGet("Chart")]
        public IActionResult Chart()
        {
            decimal totalSum = ProductDictionary.Sum(i => i.Value.Quantity * i.Value.Price);

            ListOfProductsDto = ProductDictionary.Values.ToList();
            _totalSummation = totalSum;

            return Ok(ListOfProductsDto);
        }


        [HttpPost("SelectBankAccount/{bankAccountId}")]
        public IActionResult SelectBank(int bankAccountId)
        {
            if (_totalSummation == 0)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Please REDIRECT to Chart first.");

            var bankAccount = _bankAccountRepository.FindById(bankAccountId);
            if (bankAccount == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Bank account with id={bankAccount.Id} does not exists");

            if (bankAccount.Balance < _totalSummation && bankAccount.IsActive == true)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest,
                    $"Bank account with id={bankAccount.Id} does not have sufficient balance,please refill");

            if (bankAccount.IsActive == true)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest,
                    $"Bank account with id={bankAccount.Id} is in PASSIVE STATUS , please activate it in order to proceed");


            var sales = new Sales()
            {
                Amount = _totalSummation,
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                ListOfProducts = ListOfProductsDto,
                DateCreated = DateTime.Now,
            };

            _checkoutRepository.Create(sales);

            _bankTransactionRepository.Create(new BankTransaction()
            {
                Action = ActionCall.Terheqje,
                Amount = _totalSummation,
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                DateCreated = DateTime.Now,
            });

            var mappedSale = _mapper.Map<Sales, SalesDTO>(sales);

            ListOfProductsDto.Clear();
            ProductDictionary.Clear();
            _totalSummation = 0;

            throw new HttpStatusCodeException(HttpStatusCode.OK, "Purchase was made successfully");

        }

        [HttpGet("GetSales")]
        public async Task<IActionResult> GetSales()
        {
            var data = await _checkoutRepository.GetTransactions();
            return Ok(data);
        }
    }

}