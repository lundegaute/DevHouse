using DevHouse.DTO;
using DevHouse.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateProjectTypeExample : IExamplesProvider<AddProjectTypeDTO> {
        public AddProjectTypeDTO GetExamples() {
            return new AddProjectTypeDTO {
                Name = "Arcane Ledger System"
            };
        }
    }
    public class UpdateProjectTypeExample : IExamplesProvider<UpdateProjectTypeDTO> {
        public UpdateProjectTypeDTO GetExamples() {
            return new UpdateProjectTypeDTO {
                Id = 1,
                Name = "Infernal Bug Exorcism"
            };
        }
    }
}