using System;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using AccountManagement.Mapping;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AccountManagement.Data.DTO;
using Mapper = AccountManagement.Mapping.Mapper;
using Microsoft.AspNetCore.Authorization;
using AccountManagement.ErrorHandling;
using System.Net;

namespace AccountManagement.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyController(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public IActionResult Create(CurrencyDto request)
        {
            var currency = _mapper.Map<Currency>(request);
            var succeed = _currencyRepository.Create(currency);

            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Currency was created successfully");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error creating the currency");


        }

        [HttpGet("GetCurrency/{id}")]
        public IActionResult GetCurrency(int id)
        {
            var currency = _currencyRepository.FindById(id);
            if (currency == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound,$"Currency with id={id} does NOT exist");
            return Ok(currency);
        }


        [HttpGet("GetCurrencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await _currencyRepository.GetCurrencies();
            return Ok(currencies);
        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var currency = _currencyRepository.FindById(id);
            if (currency == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Currency with id={id} does NOT exist");
            var succeed = _currencyRepository.Delete(currency);
            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Currency was deleted successfully");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error deleting the currency");

        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, CurrencyDto request)
        {
            var existingCurrency = _currencyRepository.FindById(id);
            if (existingCurrency == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Currency with id={id} does NOT exist");

            existingCurrency = _mapper.Map(request, existingCurrency);

   
                var succeed = _currencyRepository.Update(existingCurrency);
                if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Currency was updated successfully");
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error updating the currency");

        }

    }
}
