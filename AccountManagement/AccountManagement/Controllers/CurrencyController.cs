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

namespace AccountManagement.Controllers
{
    [Route("api/currencies")]
    [ApiController]
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

            try
            {
                var succeed = _currencyRepository.Create(currency);

                return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCurrency/{id}")]
        public IActionResult GetCurrency(int id)
        {
            var currency = _currencyRepository.FindById(id);
            if (currency == null) return NotFound("Currency does NOT exist");
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
            var succeed = _currencyRepository.Delete(currency);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, CurrencyDto request)
        {
            var existingCurrency = _currencyRepository.FindById(id);
            if (existingCurrency == null) return NotFound($"Currency with id={id} does NOT exist ");

            existingCurrency = _mapper.Map(request, existingCurrency);

            try
            {
                var succeed = _currencyRepository.Update(existingCurrency);
                return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //====================================MAY BE DELETED============================
        [HttpGet("PrintDetailed")]
        public IActionResult FindAll()
        {
            return Ok(_currencyRepository.FindAll());
        } //====================================MAY BE DELETED============================


    }
}
