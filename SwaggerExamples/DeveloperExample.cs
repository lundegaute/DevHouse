using DevHouse.Models;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateDeveloperExample : IExamplesProvider<AddDeveloperDTO> {
        public AddDeveloperDTO GetExamples() {
            return new AddDeveloperDTO {
                FirstName = "Archmage Recursionus",
                LastName = "The Unwise",
                RoleId = 1,
                TeamId = 1
            };
        }
    }
    public class UpdateDeveloperExample : IExamplesProvider<UpdateDeveloperDTO> {
        public UpdateDeveloperDTO GetExamples() {
            return new UpdateDeveloperDTO {
                Id = 1,
                FirstName = "Lord Fibonacci",
                LastName = "The Golden",
                RoleId = 2,
                TeamId = 2
            };
        }
    }
}