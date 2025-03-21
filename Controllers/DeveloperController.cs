using DevHouse.Models;
using DevHouse.Services;
using DevHouse.DTO;
using DevHouse.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authorization;


namespace DevHouse.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeveloperController : ControllerBase {
        private readonly DeveloperService _developerService;
        public DeveloperController(DeveloperService developerService) {
            _developerService = developerService;
        }

        /// <summary>
        /// Get all developers
        /// </summary>
        /// <response code="200">Returns all developers</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<Developer>> GetDevelopers() {
            try {
                var developers = await _developerService.GetDevelopers();
                return Ok(developers);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a single developer by Id
        /// </summary>
        /// <param name="id">Only Id > 0 is required to retrieve data</param>
        /// <response code="200">Returns requested developer</response>
        /// <response code="404">No developer found with Id</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Developer>> GetDeveloper(int id) {
            try {
                var developer = await _developerService.GetDeveloper(id);
                return Ok(developer);
            } catch (IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Add a new developer to the database
        /// </summary>
        /// <param Developer="FirstName, LastName, RoleId and TeamId is needed"></param>
        /// <param LastName="LastName must be unique"></param>
        /// <response code="201">Returns the newly created developer</response>
        /// <response code="400">Bad request: Developer already in database</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(AddDeveloperDTO), typeof(CreateDeveloperExample))]
        public async Task<ActionResult<Developer>> AddDeveloper([FromBody] AddDeveloperDTO developer) {
            try {
                var newDeveloper = await _developerService.AddDeveloper(developer);
                return CreatedAtAction(nameof(GetDeveloper), new { id = newDeveloper.Id }, newDeveloper);
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch(KeyNotFoundException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Update a developer in the database by Id
        /// </summary>
        /// <param name="id">Id > 0 is required</param>
        /// <param name="developer">LastName must be unique</param>
        /// <resonse code="204">No content</resonse>
        /// <response code="400">Bad request: Id must be greater than 0 and URL Id must match request body Id</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(UpdateDeveloperDTO), typeof(UpdateDeveloperExample))]
        public async Task<ActionResult> UpdateDeveloper(int id, [FromBody] UpdateDeveloperDTO developer) {
            try {
                await _developerService.UpdateDeveloper(id, developer);
                return NoContent();
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch (KeyNotFoundException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a developer from the database by Id
        /// </summary>
        /// <param name="id">Only Id from the URL is needed. Id > 0 is required and must match an Id in the database</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeveloper(int id) {
            try {
                await _developerService.DeleteDeveloper(id);
                return NoContent();
            } catch (IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch (KeyNotFoundException e) {
                return NotFound(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}