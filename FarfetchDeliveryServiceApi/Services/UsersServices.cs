using FarfetchDeliveryServiceApi.Models;
using FarfetchDeliveryServiceApi.Services.Interfaces;
using FarfetchDeliveryServiceRepository.Entity;
using FarfetchDeliveryServiceSqlServerRepository.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FarfetchDeliveryServiceApi.Services
{
    /// <summary>
    /// Class responsible to manage users
    /// </summary>
    public class UsersServices : IUsersServices
    {
        private readonly byte[] _key;
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UsersServices(IConfiguration configuration, IUsersRepository usersRepository)
        {
            _key = Encoding.ASCII.GetBytes(configuration.GetSection("Token").Value);

            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Validate a user autentication and return a token to use in requests
        /// </summary>
        /// <param name="user">User's data</param>
        /// <returns>Token</returns>
        public string Authenticate(User user)
        {
            Users userEntity = _usersRepository.GetByLogin(user.Login).Result;

            if (userEntity == null || ValidateUserPassword(user.Password, userEntity.Password))
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userEntity.Login),
                    new Claim(ClaimTypes.Role, userEntity.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Validate the user password
        /// </summary>
        /// <param name="password">Password to validate</param>
        /// <param name="actual">Actual ppassword</param>
        /// <returns>Password encrypted</returns>
        private bool ValidateUserPassword(string password, string actual)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);

            data = new SHA256Managed().ComputeHash(data);

            password = Encoding.ASCII.GetString(data);

            password = password.Replace(@"\\", @"\");
            actual = actual.Replace(@"\\", @"\");

            return password.Equals(actual);
        }
    }
}
