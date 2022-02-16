using Api.Shop.Apps.AdminApi.DTOs;
using Api.Shop.Apps.AdminApi.DTOs.ProductDtos;
using Api.Shop.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Shop.Apps.AdminApi.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryRepository _categoryRepository;


        public ProductsController(IProductRepository productRepository, IMapper mapper, IWebHostEnvironment env, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _env = env;
            _categoryRepository = categoryRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductPostDto postDto)
        {
            if (await _productRepository.IsExistAsync(x => x.Name.ToLower().Trim() == postDto.Name.ToLower().Trim()))
            {
                return StatusCode(409);
            }
            if (!await _categoryRepository.IsExistAsync(x => x.Id == postDto.CategoryId))
            {
                return StatusCode(404);
            }
           


            Product product = new Product
            {
                CostPrice = postDto.CostPrice,
                CategoryId = postDto.CategoryId,
                Name = postDto.Name,
                SalePrice = postDto.SalePrice,
                Image = postDto.ImageFile.SaveImg(_env.WebRootPath, "assets/img")
            };

            await _productRepository.AddAsync(product);
           await _productRepository.CommitAsync();

            return StatusCode(201, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _productRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "Category");
            if (product == null)
            {
                return StatusCode(404);
            }
            ProductGetDto getDto = _mapper.Map<ProductGetDto>(product);
            return Ok(getDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var query = _productRepository.GetAll(x => !x.IsDeleted,"Category");
            ListDto<ProductListItemDto> listDto = new ListDto<ProductListItemDto>
            {
                TotalCount = query.Count(),
                Items = query.Skip((page - 1) * 8).Take(8).Select(x => new ProductListItemDto
                {
                    Category = new CategoryInProductListItemDto
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name
                    },
                    CostPrice = x.CostPrice,
                    Id = x.Id,
                    SalePrice = x.SalePrice,
                    Name = x.Name,
                    Profit = x.SalePrice - x.CostPrice
                }).ToList()
            };
            return Ok(listDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductPostDto postDto)
        {
            
            Product product =await _productRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "Category");
            if (product == null)
            {
                return StatusCode(404);
            }
            
            if (await _categoryRepository.GetAsync(x => x.Id == postDto.CategoryId)==null)
            {
                return StatusCode(404);
            }
            if (await _categoryRepository.IsExistAsync(x => x.Id != id && x.Name.ToLower().Trim() == postDto.Name.ToLower().Trim()))
            {
                return StatusCode(409);
            }
            if (postDto.ImageFile != null)
            {
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img", product.Image);
                product.Image = postDto.ImageFile.SaveImg(_env.WebRootPath, "assets/img");
            }

            product.Name = postDto.Name;
            product.ModifiedAt = DateTime.UtcNow;
            product.CategoryId = postDto.CategoryId;
            product.CostPrice = postDto.CostPrice;
            product.SalePrice = postDto.SalePrice;
            await _productRepository.CommitAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _productRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "Category");
            if (product == null)
            {
                return StatusCode(404);
            }

            product.IsDeleted = true;
            product.ModifiedAt = DateTime.UtcNow;
            await _productRepository.CommitAsync();
            return NoContent();
        }

    }
}
