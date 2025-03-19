using DevHouse.Models;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateRoleExample : IExamplesProvider<AddRoleDTO> {
        public AddRoleDTO GetExamples() {
            return new AddRoleDTO {
                Name = "Backend Developer"
            };
        }
    }
    public class UpdateRoleExample : IExamplesProvider<UpdateRoleDTO> {
        public UpdateRoleDTO GetExamples() {
            return new UpdateRoleDTO {
                Id = 1,
                Name = "Frontend Developer"
            };
        }
    }
}