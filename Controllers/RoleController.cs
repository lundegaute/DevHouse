using Microsoft.AspNetCore.Mvc;
using DevHouse.Services;
using DevHouse.Models;
using DevHouse.SwaggerExamples;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace DevHouse.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase {
        private readonly RoleService _roleService;
        public RoleController(RoleService roleService) {
            _roleService = roleService;
        }

        /// <summary>
        /// Returns a list of all Roles
        /// </summary>
        /// <response code="200">Returns a list of all roles</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult> GetRoles() {
            try {
                var roles = await _roleService.GetRoles();
                return Ok(roles);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Returns a sing role by Id
        /// </summary>
        /// <param name="id">Specific Id to retrieve</param>
        /// <response code="200">Returns requested role</response>
        /// <response code="404">No role found with Id</response>
        /// <response code="400">Bad request: Id must be greater than 0</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id) {
            try {
                var role = await _roleService.GetRole(id);
                if ( role is null) {
                    return NotFound($"No Role found with id: {id}");
                }
                return Ok(role);
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new role to the database
        /// </summary>
        /// <param name="role">Only the name of the role is necessary in the request body</param>
        /// <response code="201">Returns the newly created role</response>
        /// <response code="400">Bad request: Name cannot be null, empty or is already in database</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(AddRoleDTO), typeof(CreateRoleExample))]
        public async Task<ActionResult<Role>> AddRole([FromBody] AddRoleDTO role) {
            try {
                var newRole = await _roleService.AddRole(role.Name);
                return CreatedAtAction(nameof(GetRole), new { id = newRole.Id }, newRole);
            } catch (ArgumentException e) {
                return BadRequest(e.Message);
            } catch(InvalidOperationException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Updates a role in the database by Id
        /// </summary>
        /// <param name="role">Only name and id is needed. Id from URL must match Id from body</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(Role), typeof(UpdateRoleExample))]
        public async Task<ActionResult<Role>> UpdateRole(int id, UpdateRoleDTO role) {
            try {
                await _roleService.UpdateRole(id, role);
                return NoContent();
                
            } catch (ArgumentException e) { 
                return BadRequest(e.Message);

            } catch (IndexOutOfRangeException e) {
                return BadRequest(e.Message);

            } catch (InvalidOperationException e) {
                return BadRequest(e.Message);

            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
        
        /// <summary>
        /// Deletes a role from the database by Id
        /// </summary>
        /// <param name="id">Only Id from url is needed</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(int id) {
            try {
                await _roleService.DeleteRole(id);
                return NoContent();
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            } catch(IndexOutOfRangeException e) {
                return BadRequest(e.Message);
            } catch (HttpRequestException) {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}