﻿using System;
using AccountManagement.Contracts;
using AccountManagement.Data.Model;
using AccountManagement.Data;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AccountManagement.Data.DTO;

namespace AccountManagement.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpPost("Create")]
        public IActionResult Create(CategoryDto request)
        {
            var category = _mapper.Map<Category>(request);


            try
            {
                var succeed = _categoryRepository.Create(category);
                return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.FindById(id);
            if (category == null) return NotFound($"Category with id={id} does NOT exist");
            return Ok(category);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var currencies = await _categoryRepository.GetCategories();
            return Ok(currencies);
        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.FindById(id);
            if (category == null) return NotFound($"Category with id={id} does NOT exist");

            var succeed = _categoryRepository.Delete(category);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, CategoryDto request)
        {
            var category = _categoryRepository.FindById(id);
            if (category == null) return NotFound($"Category with id={id} does NOT exist");

            category = _mapper.Map(request, category);
            try
            {
                var succeed = _categoryRepository.Update(category);
                return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
