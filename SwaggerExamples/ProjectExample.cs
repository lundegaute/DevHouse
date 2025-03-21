using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateProjectExample : IExamplesProvider<AddProjectDTO> {
        public AddProjectDTO GetExamples() {
            return new AddProjectDTO {
                Name = "The Tome of Infinite Queries",
                TeamId = 1,
                ProjectTypeId = 1
            };
        }
    }
    public class UpdateProjectExample : IExamplesProvider<UpdateProjectDTO> {
        public UpdateProjectDTO GetExamples() {
            return new UpdateProjectDTO {
                Id = 1,
                Name = "The Holy Debugging Crusade",
                TeamId = 2,
                ProjectTypeId = 2
            };
        }
    }
}