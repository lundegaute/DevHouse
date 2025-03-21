using DevHouse.Models;
using DevHouse.DTO;
using DevHouse.Services;
using DevHouse.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authorization;

namespace DevHouse.Controller {
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase {
        private readonly ProjectService _projectService;
        public ProjectController(ProjectService projectService) {
            _projectService = projectService;
        }
        
        /// <summary>
        /// Returns a list of all Projects
        /// </summary>
        /// <response code="200">Returns a list of all projects</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult> GetProjects() {
            try {
                var projects = await _projectService.GetProjects();
                return Ok(projects);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Returns a single Project by Id
        /// </summary>
        /// <param name="id">Id > 0 is required</param>
        /// <response code="200">Returns requested project</response>
        /// <response code="404">No project found with Id</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id) {
            try {
                var project = await _projectService.GetProject(id);
                return Ok(project);
            } catch ( IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch ( KeyNotFoundException e) {
                return NotFound(e.Message);
            } catch ( HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new Project
        /// </summary>
        /// <param name="project">A unique name is required</param>
        /// <param name="project">A valid team Id and project type Id is required</param>
        /// <response code="201">Returns the newly created project</response>
        /// <response code="400">Bad request: Name cannot be null, empty or is already in database</response>
        /// <response code="404">Not found: Team or Project Type not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(AddProjectDTO), typeof(CreateProjectExample))]
        public async Task<ActionResult<Project>> AddProject([FromBody] AddProjectDTO project) {
            try {
                var newProject = await _projectService.AddProject(project);
                return CreatedAtAction(nameof(GetProject), new { id = newProject.Id}, newProject);
            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);
            } catch (KeyNotFoundException e) {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Updates a Project By Id
        /// </summary>
        /// <param name="id">Id > 0 is required and must match request body id</param>
        /// <param name="project">Name cannot be null, empty or a duplicate</param>
        /// <param name="project">Team Id and Project Type Id must be valid</param>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request: Id must be greater than 0 and URL Id must match request body Id</response>
        /// <response code="404">Not found: Project not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(UpdateProjectDTO), typeof(UpdateProjectExample))]
        public async Task<ActionResult> UpdateProject(int id, [FromBody] UpdateProjectDTO project) {
            try {
                await _projectService.UpdateProject(id, project);
                return NoContent();
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch (KeyNotFoundException e) {
                return NotFound(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Deletes a Project by Id
        /// </summary>
        /// <param name="id">Id > 0 is required</param>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="404">Not found: Project not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id) {
            try {
                await _projectService.DeleteProject(id);
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