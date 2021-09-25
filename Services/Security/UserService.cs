using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpsonApp.Models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SimpsonApp.Exceptions;

namespace SimpsonApp.Services.Security
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }


        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User can't log in",
                    IsSuccess = false,
                    Errors = new List<string> {"There is no user with that Email address" }
                };
            }

            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: configuration["AuthSettings:Issuer"],
                audience: configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NotModelException("model is null");
            }

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "User did not create",
                    IsSuccess = false,
                    Errors = new List<string> () { "Confirm password doesn't match the password" }
                };

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> CreateRoleAsync(CreateRoleViewModel model)
        {

            var identityRole = new IdentityRole()
            {
                Name = model.Name,
                NormalizedName = model.NormalizedName
            };

            var result = await roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Role created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "Role did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }


        public async Task<UserManagerResponse> CreateUserRoleAsync(CreateUserRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return new UserManagerResponse
                {
                    Message = "Role does not exist",
                    IsSuccess = false
                };
            }
            else
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "user does not exist",
                        IsSuccess = false
                    };
                }

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    return new UserManagerResponse
                    {
                        Message = "user has role already",
                        IsSuccess = false
                    };
                }
                else
                {
                    var result = await userManager.AddToRoleAsync(user, role.Name);

                    if (result.Succeeded)
                    {
                        return new UserManagerResponse
                        {
                            Message = "Role assigned",
                            IsSuccess = true
                        };
                    }

                    return new UserManagerResponse
                    {
                        Message = "something went wrong",
                        IsSuccess = false
                    };
                }
            }
        }
    }
}
