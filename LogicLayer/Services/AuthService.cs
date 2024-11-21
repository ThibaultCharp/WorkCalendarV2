using LogicLayer.IRepos;
using Microsoft.Extensions.Configuration;  // Make sure to include this namespace
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LogicLayer.Entitys;

namespace LogicLayer.Services
{
    public class AuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IConfiguration _configuration;

        // Constructor that accepts IConfiguration
        public AuthService(IAuthRepo authRepo, IConfiguration configuration)
        {
            _authRepo = authRepo;
            _configuration = configuration;
        }

        public void RegisterUser(string name, string email, string password, int roleId)
        {
            // Check if the user already exists
            var existingUser = _authRepo.GetUserByEmail(email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create new user
            User newUser = new User
            {
                name = name,
                email = email,
                password = hashedPassword,
                role = new Entities.Role { id = roleId }
            };

            // Save the new user to the database
            _authRepo.AddUser(newUser);
        }

        // Login a user and return a JWT token
        public string LoginUser(string email, string password)
        {
            // Check if the user exists
            var user = _authRepo.GetUserByEmail(email);
            if (user == null)
            {
                return null; // Invalid user
            }

            // Verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.password);
            if (!isPasswordValid)
            {
                return null; // Invalid password
            }

            // Generate the JWT token
            return GenerateJwtToken(user);
        }

        // Generate the JWT token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.name),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.email),
                new System.Security.Claims.Claim("role", user.role.name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));  // Access configuration directly here
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],  // Access configuration for issuer
                audience: _configuration["Jwt:Audience"],  // Access configuration for audience
                claims: claims,
                expires: DateTime.Now.AddMinutes(1), // Token expiration time (e.g., 1 hour)
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
