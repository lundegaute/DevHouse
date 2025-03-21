using DevHouse.Models;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateTeamExample : IExamplesProvider<AddTeamDTO> {
        public AddTeamDTO GetExamples() {
            return new AddTeamDTO {
                Name = "InterDimensional Vampire Hunters"
            };
        }
    }
    public class UpdateTeamExample : IExamplesProvider<UpdateTeamDTO> {
        public UpdateTeamDTO GetExamples() {
            return new UpdateTeamDTO {
                Id = 1,
                Name = "Electromagnetic Dragonslayers"
            };
        }
    }
}