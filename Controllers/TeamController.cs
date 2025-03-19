using DevHouse.Models;
using DevHouse.Services;
using DevHouse.DTO;
using DevHouse.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase {
        private readonly TeamService _teamService;
        public TeamController(TeamService teamService ) {
            _teamService = teamService;
        }

        /// <summary>
        /// Returns a list of all Teams
        /// </summary>
        /// <response code="200">Returns a list of all teams</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult> GetTeams() {
            try {
                var teams = await _teamService.GetTeams();
                return Ok(teams);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Returns a single team by Id
        /// </summary>
        /// <param name="id">Only Id > 0 is required to retrieve data</param>
        /// <response code="200">Returns requested team</response>
        /// <response code="404">No team found with Id</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id) {
            try {
                var team = await _teamService.GetTeam(id);
                return Ok(team);
            } catch (IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new team to the database
        /// </summary>
        /// <param name="team">Only name is required in the request body</param>
        /// <response code="201">Returns the newly created team</response>
        /// <response code="400">Bad request: Name cannot be null, empty or is already in database</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(AddTeamDTO), typeof(CreateTeamExample))]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] AddTeamDTO team) {
            try {
                var newTeam = await _teamService.AddTeam(team.Name);
                return CreatedAtAction(nameof(GetTeam), new { id = newTeam.Id }, newTeam);
            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a team in the database by Id
        /// </summary>
        /// <param name="id">Id required through URL and must match request body id</param>
        /// <param name="team">Name cannot be null or empty</param>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request: Id must be greater than 0 and URL Id must match request body Id</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(UpdateTeamDTO), typeof(UpdateTeamExample))]
        public async Task<ActionResult> UpdateTeam(int id, [FromBody] UpdateTeamDTO team) {
            try {
                await _teamService.UpdateTeam(id, team);
                return NoContent();
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch (InvalidOperationException e ) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a team from the database by Id
        /// </summary>
        /// <param name="id">Only Id from the URL is needed. Id > 0 is required and must match an Id in the database</param>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="400">Bad request: Team not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id) {
            try {
                await _teamService.DeleteTeam(id);
                return NoContent();
            } catch (IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}