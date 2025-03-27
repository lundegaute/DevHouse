using DevHouse.Models;
using DevHouse.Services;
using DevHouse.DTO;
using DevHouse.SwaggerExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace DevHouse.Controller {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly AuthService _authService;
        private readonly IMemoryCache _cache;

        public AuthController(AuthService authService, IMemoryCache cache) {
            _authService = authService;
            _cache = cache;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [SwaggerRequestExample(typeof(RegisterDTO), typeof(RegisterUserExample))]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register) {
            string cacheKey = $"RegisterAttempts_{HttpContext.Connection.RemoteIpAddress}";
            int requestCount = _cache.GetOrCreate(cacheKey, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return 0;
            });

            if (requestCount >= 10) {
                return StatusCode(429, "Too many requests. Please try again later.");
            }

            _cache.Set(cacheKey, requestCount + 1);

            try {
                if ( await _authService.RegisterUserAsync(register.Username, register.Password)) {
                    return Ok("Registration Successful");
                }
                return BadRequest("Registration failed");
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