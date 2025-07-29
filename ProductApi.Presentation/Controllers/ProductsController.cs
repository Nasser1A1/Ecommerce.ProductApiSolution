using eCommerce.SharedLib.ResponseT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using System.Diagnostics;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct _productRepo) : ControllerBase
    {
        [HttpGet("debug-token")]
        [AllowAnonymous]
        public IActionResult DebugToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            return Ok(authHeader);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepo.FindByIdAsync(id);
            if (product is null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            var (productDto, _) = ProductConversion.FromEntity(product, null);
            return productDto! is not null ? Ok(productDto) : NotFound("No Product found");
        }

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productRepo.GetAllAsync();
            if (products is null || !products.Any())
            {
                return NotFound("There are no products at the moment");
            }
            // convert IEnumerable<Product> to IEnumerable<ProductDto>
            var (_, productDtos) = ProductConversion.FromEntity(null, products);
            return productDtos!.Any() ? Ok(productDtos) : NotFound("No Products found");
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // convert ProductDto to Product entity
            var product = productDto.ToEntity();
            var response = await _productRepo.CreateAsync(product);

            return response.Flag ? CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto) : BadRequest(response);
        }


        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = productDto.ToEntity();
            var response = await _productRepo.UpdateAsync(product);
            return response.Flag ? Ok(response) : BadRequest(response);
        }


        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = productDto.ToEntity();
            var response = await _productRepo.DeleteAsync(product);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

    }
}
