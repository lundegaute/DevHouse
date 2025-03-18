using DevHouse.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class CreateRoleExample : IExamplesProvider<Role> {
        public Role GetExamples() {
            return new Role {
                Name = "Backend Developer"
            };
        }
    }
    public class UpdateRoleExample : IExamplesProvider<Role> {
        public Role GetExamples() {
            return new Role {
                Id = 1,
                Name = "Frontend Developer"
            };
        }
    }
}