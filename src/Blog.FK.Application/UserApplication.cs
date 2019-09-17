using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Configurations;
using Blog.FK.Domain.Entities;
using Blog.FK.Domain.Extensions;
using Blog.FK.Domain.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;

namespace Blog.FK.Application
{
    public class UserApplication : BaseApplication<User>, IUserApplication
    {
        #region "  Services & Repositories  "

        private readonly IUserRepository _userRepository;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;

        #endregion

        #region "  Constructors  "

        public UserApplication(IUserRepository userRepository, TokenConfigurations tokenConfigurations,
            SigningConfigurations signingConfigurations) : base(userRepository)
        {
            _userRepository = userRepository;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
        }

        #endregion

        #region "  IUserApplication  "

        public async Task<Token> AuthenticateAsync(User user)
        {
            var result = await _userRepository.SearchAsync(u => u.Email == user.Email && u.Password == user.Password.Cript());

            var _user = result.FirstOrDefault();

            if (_user != null)
            {
                var identity = new ClaimsIdentity(
                    new GenericIdentity(_user.Id.ToString(), "Login"),
                    new List<Claim>()
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, _user.Name)
                    }
                );

                DateTime createdAt = DateTime.Now;
                DateTime expiresAt = createdAt.AddSeconds(_tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();

                var securityToken = handler.CreateToken(new SecurityTokenDescriptor()
                {
                    Issuer = null,
                    Audience = null,
                    Subject = identity,
                    NotBefore = createdAt,
                    Expires = expiresAt,
                    SigningCredentials = _signingConfigurations.SigningCredentials
                });

                string token = handler.WriteToken(securityToken);

                return new Token()
                {
                    AccessToken = token,
                    Authenticated = true,
                    CreatedAt = createdAt,
                    ExpiresAt = expiresAt
                };
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
