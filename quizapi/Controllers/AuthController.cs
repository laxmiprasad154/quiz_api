using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using quizapi.Business_Logic_Layer.DTO;
using quizapi.Data_Access_Layer.Entities;
using quizapi.Data_Access_Layer.Repository.Interface;
using quizapi.Infrastructure;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using quizapi.Data_Access_Layer.context;

namespace quizapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private quizdbcontext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;
  

        public AuthController(quizdbcontext context, IConfiguration configuration, IMapper mapper, IUserRepo userRepo)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
            this.userRepo = userRepo;
            
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(AddAuthUserLoginDTO loginModel)
        {

           var user = context.Users.Include(x => x.UserRole).FirstOrDefault(x => x.Email == loginModel.UserEmail);

            if (user == null)
                return Unauthorized("Invalid Username or Password!");

            string hashedPassword = HashPassword(loginModel.UserPassword);
            if (BCrypt.Net.BCrypt.Verify(loginModel.UserPassword, hashedPassword))
            {

                var token = JWT.GenerateToken(new Dictionary<string, string> {
                { ClaimTypes.Role, user.UserRole.UserRolesName  },
                { "RoleId", user.UserRole.UserRoleId.ToString() },
                {JwtClaimTypes.PreferredUserName, user.UserName },
                { JwtClaimTypes.Id, user.UserId.ToString() },
                { JwtClaimTypes.Email, user.Email}
            }, configuration["JWT:Key"]);



                return Ok(new AddAuthResponseDTO { token = token, UserName = user.UserName });
            }
            else
            {
                return Unauthorized("Invalid Username or Password");
            }
        }
        [Route("Registration")]
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddUserRequestDTO addUserRequestDTO)
        {

            // Check if a user with the same email already exists
            var existingUser = await userRepo.GetByEmailAsync(addUserRequestDTO.Email);
            if (existingUser != null)
            {
                // Return an error response indicating that the email is already registered
                return BadRequest("Email is already registered.");
            }
            //Map DTO to Domain Model           
            var userEntity = mapper.Map<User>(addUserRequestDTO);
            userEntity.Password = HashPassword(addUserRequestDTO.Password);



            await userRepo.CreateAsync(userEntity);
            // var users = mapper.Map<UserDTO>(userEntity);

            return Ok("Registration Successful");
        }
        private  string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
    }
}
