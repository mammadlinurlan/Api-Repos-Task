using Api.Shop.Apps.AdminApi.DTOs;
using Api.Shop.Apps.AdminApi.DTOs.CategoryDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Shop.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryPostDto postDto)
        {
            if (await _categoryRepository.IsExistAsync(x => x.Name.ToLower().Trim() == postDto.Name.ToLower().Trim()))
            {
                return StatusCode(409);
            }
            Category category = new Category
            {
                Name = postDto.Name,

            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();
            return StatusCode(201, category);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id && !x.IsDeleted,"Products");


            if (category == null)
            {
                return StatusCode(404);
            }

            CategoryGetDto getDTO = new CategoryGetDto
            {
                CreatedTime = category.CreatedAt,
                Id = category.Id,
                ModifiedTime = category.ModifiedAt,
                Name = category.Name,
                 ProductsCount = category.Products.Count()

            };
            return Ok(getDTO);
        }

        [HttpGet("")]
        public  IActionResult GetAll(int page = 1)
        {
            var query = _categoryRepository.GetAll(x => !x.IsDeleted);

            ListDto<CategoryListItemDto> listDto = new ListDto<CategoryListItemDto>
            {
                TotalCount = query.Count(),
                Items = query.Skip((page - 1) * 8).Take(8).Select(x => new CategoryListItemDto { Id = x.Id, Name = x.Name }).ToList()
            };

            return Ok(listDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromForm]CategoryPostDto postDto)
        {

            Category category =await _categoryRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null)
            {
                return StatusCode(404);
            }

            if (await _categoryRepository.IsExistAsync(x => x.Id != id && x.Name.ToLower().Trim() == postDto.Name.ToLower().Trim()))
            {
                return StatusCode(409);
            }



          
            category.Name = postDto.Name;
            category.ModifiedAt = DateTime.UtcNow;
            await _categoryRepository.CommitAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async  Task<IActionResult> Delete(int id)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null)
            {
                return StatusCode(404);
            }

            _categoryRepository.Remove(category);
            //category.IsDeleted = true;
            //category.ModifiedAt = DateTime.UtcNow;
           await _categoryRepository.CommitAsync();
            return NoContent();

        }
    }
}
