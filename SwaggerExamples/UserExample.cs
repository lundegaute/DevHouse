using DevHouse.Models;
using DevHouse.DTO;
using Swashbuckle.AspNetCore.Filters;

namespace DevHouse.SwaggerExamples {
    public class RegisterUserExample : IExamplesProvider<RegisterDTO> {
        public RegisterDTO GetExamples() {
            return new RegisterDTO {
                Username = "Batman",
                Password = "Justice"

            };
        }
    }
    public class LoginUserExample : IExamplesProvider<LoginDTO> {
        public LoginDTO GetExamples() {
            return new LoginDTO {
                Username = "Batman",
                Password = "Justice"
            };
        }
    }
}