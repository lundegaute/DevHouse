using DevHouse.Models;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateDeveloperExample : IExamplesProvider<AddDeveloperDTO> {
        public AddDeveloperDTO GetExamples() {
            return new AddDeveloperDTO {
                FirstName = "Bruce",
                LastName = "Wayne",
                RoleId = 1,
                TeamId = 1
            };
        }
    }
    public class UpdateDeveloperExample : IExamplesProvider<UpdateDeveloperDTO> {
        public UpdateDeveloperDTO GetExamples() {
            return new UpdateDeveloperDTO {
                Id = 1,
                FirstName = "Clark",
                LastName = "Kent",
                RoleId = 2,
                TeamId = 2
            };
        }
    }
}