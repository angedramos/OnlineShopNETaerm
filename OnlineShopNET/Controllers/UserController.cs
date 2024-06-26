using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OnlineShopNET.Application.DTOs;
using OnlineShopNET.Domain.Config;
using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Persistence.Interfaces;
using OnlineShopNET.JwtService;
using System.Security.Claims;

namespace OnlineShopNET.Controllers
{
    [Route("api/v1/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userService;
        public UserController( IMapper mapper, IUserRepository userService, IJwtService jwtService)
        {
            _mapper = mapper;
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if(authorizationHeader == null)
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

            if (roleID == (int)UserRoleType.Admin)
            {
                var usersList = await _userService.GetUsers();
                return Ok(usersList);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, Constant_Messages.ALLOWED_ADMINS_ONLY);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user, [FromServices] IValidator<User> validator)
        {
            UserDTO setUser = new UserDTO
            {
                username = user.username,
                password = user.password,
                email = user.email,
                role_type = user.role_type,
                created_at = DateTime.Now
            };
            var getUser = _mapper.Map<User>(setUser);
            var validationResult = validator.Validate(getUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var newUser = await _userService.CreateUser(getUser);
            if(newUser == null)
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.DUPLICATED_USERNAME_EMAIL);
            return Ok(newUser); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO model)
        {
            var checkUser = _userService.GetUserByUsernameandPassword(model.username, model.password);

            if(checkUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constant_Messages.INVALID_CREDENTIALS);
            }

            string token = _jwtService.GenerateJwtToken(checkUser);
            return StatusCode(StatusCodes.Status200OK,token);

        }
    }
}
