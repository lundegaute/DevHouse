using DevHouse.Models;
using DevHouse.Services;
using DevHouse.DTO;
using DevHouse.SwaggerExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace DevHouse.Controller {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [SwaggerRequestExample(typeof(RegisterDTO), typeof(RegisterUserExample))]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register) {
            try {
                if ( await _authService.RegisterUserAsync(register.Username, register.Password)) {
                    return Ok("Registration Successful");
                }
                return BadRequest("Registration failed. User already exists.");
            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
         }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [SwaggerRequestExample(typeof(LoginDTO), typeof(LoginUserExample))]
        public async Task<IActionResult> Login([FromBody]LoginDTO login) {
            try {
                if ( await _authService.ValidateUserAsync(login.Username, login.Password)) {
                    var user = await _authService.GetUser(login.Username);
                    var token = _authService.GenerateToken(user);
                    return Ok(token);
                } else {
                    return BadRequest("Invalid username or password");
                }
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}