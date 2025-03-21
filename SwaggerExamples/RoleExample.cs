using DevHouse.Models;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateRoleExample : IExamplesProvider<AddRoleDTO> {
        public AddRoleDTO GetExamples() {
            return new AddRoleDTO {
                Name = "Wizard of Infinite Loops"
            };
        }
    }
    public class UpdateRoleExample : IExamplesProvider<UpdateRoleDTO> {
        public UpdateRoleDTO GetExamples() {
            return new UpdateRoleDTO {
                Id = 1,
                Name = "Necromancer of the Legacy Code"
            };
        }
    }
}