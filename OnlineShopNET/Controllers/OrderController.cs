using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Config;
using OnlineShopNET.Infrastructure.Services;
using OnlineShopNET.JwtService;
using System.Security.Claims;

namespace OnlineShopNET.Controllers
{
    [Authorize]
    [Route("api/v1/Orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IJwtService _jwtService;

        public OrderController(IJwtService jwtService, IOrderService orderService)
        {
            _jwtService = jwtService;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] List<ProductsListDTO> products)
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.NULL_TOKEN);
            }
            var bearerToken = _jwtService.GetTokenFromHeader(authorizationHeader);
            var validToken = _jwtService.IsValidJwtToken(bearerToken);

            if (!validToken)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.INVALID_TOKEN);
            }
            var claimsPrincipal = _jwtService.DecodeJwtToken(bearerToken);
            var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            int roleID = int.Parse(roleClaim);
            var userIDClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userID = int.Parse(userIDClaim);

            if (roleID == (int)UserRoleType.User)
            {
                try
                {
                    var order = await _orderService.PlaceOrder(userID, products);

                    return Ok(order);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            else if(roleID == (int)UserRoleType.Admin)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.ALLOWED_USERS_ONLY);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.UNLOGGED_USER);
            }
        }
    }
}

