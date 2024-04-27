using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Config;
using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Persistence.Interfaces;
using OnlineShopNET.JwtService;
using System.Security.Claims;


namespace OnlineShopNET.Controllers
{
    [Authorize]
    [Route("api/v1/Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public ProductController(IProductRepository productService, IMapper mapper, IJwtService jwtService)
        {
            _productService = productService;
            _mapper = mapper;
            _jwtService = jwtService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.NULL_TOKEN);
            }
            var bearerToken = _jwtService.GetTokenFromHeader(authorizationHeader);
            var claimsPrincipal = _jwtService.DecodeJwtToken(bearerToken);
            var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            int roleID = int.Parse(roleClaim);

            if (roleID == (int)UserRoleType.Admin || roleID == (int)UserRoleType.User)
            {
                var productsList = await _productService.GetAvailableProducts();
                return Ok(productsList);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.UNLOGGED_USER);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO product, [FromServices] IValidator<Product> validator)
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.NULL_TOKEN);
            }

            var bearerToken = _jwtService.GetTokenFromHeader(authorizationHeader);
            var claimsPrincipal = _jwtService.DecodeJwtToken(bearerToken);
            var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            int roleID = int.Parse(roleClaim);

            if (roleID == (int)UserRoleType.Admin)
            {
                ProductDTO productDTO = new ProductDTO
                {
                    product_name = product.product_name,
                    product_description = product.product_description,
                    product_price = product.product_price,
                    product_quantity = product.product_quantity,
                    categoryId = product.categoryId,
                    product_status = true
                };

                var newProduct = _mapper.Map<Product>(productDTO);
                var validationResult = validator.Validate(newProduct);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var productAdded = await _productService.CreateProduct(newProduct);
                return Ok(productAdded);
            }
            if (roleID == (int)UserRoleType.User)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.ALLOWED_ADMINS_ONLY);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.UNLOGGED_USER);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO product, [FromServices] IValidator<Product> validator)
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.NULL_TOKEN);
            }

            var bearerToken = _jwtService.GetTokenFromHeader(authorizationHeader);
            var claimsPrincipal = _jwtService.DecodeJwtToken(bearerToken);
            var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

            int roleID = int.Parse(roleClaim);

            if (roleID == (int)UserRoleType.Admin)
            {
                var checkProduct = await _productService.GetProductById(product.productID);
                if (checkProduct == null)
                    return StatusCode(StatusCodes.Status404NotFound, Constant_Messages.NON_EXISTENT_PRODUCT);
                var updateProduct = _mapper.Map<Product>(product);
                var validationResult = validator.Validate(updateProduct);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var updatedProduct = await _productService.UpdateProduct(updateProduct);

                return Ok(updatedProduct);
            }
            if (roleID == (int)UserRoleType.User)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.ALLOWED_ADMINS_ONLY);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.UNLOGGED_USER);
        }

        [HttpDelete("{productID}")]
        public async Task<IActionResult> DeleteProduct(int productID)
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.NULL_TOKEN);
            }

            var bearerToken = _jwtService.GetTokenFromHeader(authorizationHeader);
            var claimsPrincipal = _jwtService.DecodeJwtToken(bearerToken);
            var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            int roleID = int.Parse(roleClaim);

            if (roleID == (int)UserRoleType.Admin)
            {
                var checkProduct = await _productService.GetProductById(productID);
                if (checkProduct == null)
                    return StatusCode(StatusCodes.Status404NotFound, Constant_Messages.NON_EXISTENT_PRODUCT);
                var removeProduct = await _productService.DeleteProduct(checkProduct);
                return Ok(removeProduct);
            }

            if (roleID == (int)UserRoleType.User)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.ALLOWED_ADMINS_ONLY);
            }

            return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.UNLOGGED_USER);
        }

    }
}
