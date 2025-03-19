using DevHouse.DTO;
using DevHouse.Models;
using DevHouse.Services;
using DevHouse.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTypeController : ControllerBase {
        private readonly ProjectTypeService _projectTypeService;
        public ProjectTypeController(ProjectTypeService projectTypeService) {
            _projectTypeService = projectTypeService;
        }

        /// <summary>
        /// Returns a list of all Project Types
        /// </summary>
        /// <response code="200">Returns a list of all project types</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult> GetProjectTypes() {
            try {
                var projectTypes = await _projectTypeService.GetProjectTypes();
                return Ok(projectTypes);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Returns a single project type by Id
        /// </summary>
        /// <param name="id">Only Id > 0 is required to retrieve data</param>
        /// <response code="200">Returns requested project type</response>
        /// <response code="404">No project type found with Id</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectType>> GetProjectType(int id) {
            try {
                var projectType = await _projectTypeService.GetProjectType(id);
                return Ok(projectType);
            } catch (IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch (KeyNotFoundException e) {
                return NotFound(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new project type to the database
        /// </summary>
        /// <param name="projectType">Only name is required in the request body</param>
        /// <response code="201">Returns the newly created project type</response>
        /// <response code="400">Bad request: Name cannot be null, empty or is already in database</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(AddProjectTypeDTO), typeof(CreateProjectTypeExample))]
        public async Task<ActionResult<ProjectType>> AddProjectType([FromBody] AddProjectTypeDTO projectType) {
            try {
                var newProjectType = await _projectTypeService.AddProjectType(projectType);
                return CreatedAtAction(nameof(GetProjectType), new { id = newProjectType.Id }, newProjectType);
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a project type in the database by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectType"></param>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(UpdateProjectTypeDTO), typeof(UpdateProjectTypeExample))]
        public async Task<ActionResult<ProjectType>> UpdateProjectType(int id, [FromBody] UpdateProjectTypeDTO projectType) {
            try {
                await _projectTypeService.UpdateProjectType(id, projectType);
                return NoContent();
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Deletes a project type from the database by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectType( int id ) {
            try {
                await _projectTypeService.DeleteProjectType(id);
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