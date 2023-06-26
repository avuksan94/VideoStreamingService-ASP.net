using AutoMapper;
using Azure;
using BLL.Models;
using DAL.Models;
using DAL.Repo;
using DAL.Requests;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(UnitOfWork unitOfWork,IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }


        public async Task<IEnumerable<BLUser>> GetAllUsers()
        {
           var dbUsers = await _unitOfWork.UserRepository.GetAsync(includeProperties:"CountryOfResidence");
           var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);
           return blUsers;
        }

        public async Task<BLUser> GetUserById(int id)
        {
            var dbUsers = await _unitOfWork.UserRepository.GetAsync(includeProperties: "CountryOfResidence");
            var dbUser = dbUsers.FirstOrDefault(u => u.Id == id);
            var blUser = _mapper.Map<Models.BLUser>(dbUser);
            return blUser;
        }

        public async Task<BLUser> AddUser(UserRequest request)
        {
            var dbUsers = await _unitOfWork.UserRepository.GetAsync();
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);
            // Username: Normalize and check if username exists
            var normalizedUsername = request.Username.ToLower().Trim();
            if (blUsers.Any(x => x.Username.Equals(normalizedUsername)))
                throw new InvalidOperationException("Username already exists");

            // Password: Salt and hash password
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: request.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            // SecurityToken: Random security token
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            // Id: Next id
            int nextId = 1;
            if (blUsers.Any())
            {

                nextId = blUsers.Max(x => x.Id) + 1;
            }

            // New user
            var newUser = new User
            {
                CreatedAt = DateTime.Now,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Phone = request.Phone,
                IsConfirmed = false,
                SecurityToken = b64SecToken,
                PwdSalt = b64Salt,
                PwdHash = b64Hash,
                CountryOfResidenceId = request.Country
            };


            await _unitOfWork.UserRepository.InsertAsync(newUser);
            await _unitOfWork.SaveAsync();
            var user = _mapper.Map<BLUser>(newUser);

            return user;
        }

        public async Task ValidateEmail(EmailValidationRequest request)
        {
            var allUsers = await _unitOfWork.UserRepository.GetAsync();
            var target = allUsers.FirstOrDefault(x =>
                x.Username == request.Username && x.SecurityToken == request.B64SecToken);

            if (target == null)
            {
                throw new InvalidOperationException("Authentication failed");
            }
            else
            {
                target.IsConfirmed = true;
                await _unitOfWork.UserRepository.UpdateAsync(target);
                await _unitOfWork.SaveAsync();
            }
        }

        private async Task<bool> Authenticate(string username, string password)
        {
            var allUsers = await _unitOfWork.UserRepository.GetAsync();
            var target =  allUsers.Single(x => x.Username == username);

            if (!target.IsConfirmed)
                return false;

            // Get stored salt and hash
            byte[] salt = Convert.FromBase64String(target.PwdSalt);
            byte[] hash = Convert.FromBase64String(target.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash);
            
        }

        public async Task<Tokens> JwtTokens(JWTTokensRequest request)
        {
            var isAuthenticated = await Authenticate(request?.Username, request?.Password);

            if (!isAuthenticated)
                throw new InvalidOperationException("Authentication failed");

            // Get secret key bytes
            var jwtKey = _configuration["JWT:Key"];
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);

            // Create a token descriptor (represents a token, kind of a "template" for token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new System.Security.Claims.Claim[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.Name, request.Username),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    //new System.Security.Claims.Claim(ClaimTypes.Role, "User")
                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create token using that descriptor, serialize it and return it
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return new Tokens
            {
                Token = serializedToken
            };
        }


        public async Task UpdateUser(BLUser blUser)
        {
            var dbUsers = await _unitOfWork.UserRepository.GetAsync(includeProperties: "CountryOfResidence");
            var existingUser = dbUsers.FirstOrDefault(u => u.Id == blUser.Id);

            if (existingUser == null)
            {
                return;
            }

            var countryExists = await _unitOfWork.CountryRepository.GetByIDAsync(blUser.CountryOfResidenceId);
            if (countryExists == null)
            {
                throw new Exception($"Country with ID {blUser.CountryOfResidenceId} does not exist");
            }

            existingUser.Id = blUser.Id;
            existingUser.Username = blUser.Username;
            existingUser.FirstName = blUser.FirstName;
            existingUser.LastName = blUser.LastName;
            existingUser.Email = blUser.Email;
            existingUser.Phone = blUser.Phone;
            existingUser.IsConfirmed = blUser.IsConfirmed;
            existingUser.CountryOfResidenceId = blUser.CountryOfResidenceId;
            existingUser.DeletedAt = blUser.DeletedAt;

            await _unitOfWork.UserRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUser(int id)
        {
            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public void SaveUserData() => _unitOfWork.Save();

        //Potrebno mi je u Public Module

        public async Task ChangePassword(string username, string newPassword)
        {
            var allUsers = await _unitOfWork.UserRepository.GetAsync();
            var userToChangePassword = allUsers.FirstOrDefault(x =>
                x.Username == username &&
                !x.DeletedAt.HasValue);

            (var salt, var b64Salt) = GenerateSalt();

            var b64Hash = CreateHash(newPassword, salt);

            userToChangePassword.PwdHash = b64Hash;
            userToChangePassword.PwdSalt = b64Salt;

            _unitOfWork.Save();
        }

        private static (byte[], string) GenerateSalt()
        {
            // Generate salt
            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var b64Salt = Convert.ToBase64String(salt);

            return (salt, b64Salt);
        }

        private static string CreateHash(string password, byte[] salt)
        {
            // Create hash from password and salt
            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }

        public async Task<BLUser> GetConfirmedUser(string username, string password)
        {
            var dbUsers = await _unitOfWork.UserRepository.GetAsync(includeProperties: "CountryOfResidence");
            var dbUser = dbUsers.FirstOrDefault(x =>
                x.Username == username &&
                x.IsConfirmed == true);

            if (dbUser == null)
            {
                return null;
            }

            var salt = Convert.FromBase64String(dbUser.PwdSalt);
            var b64Hash = CreateHash(password, salt);

            if (dbUser.PwdHash != b64Hash) 
            {
                return null;
            }

            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }

        public async Task ConfirmEmail(string email, string securityToken)
        {
            var allUsers = await  _unitOfWork.UserRepository.GetAsync();
            var userToConfirm = allUsers.FirstOrDefault(x =>
                x.Email == email &&
                x.SecurityToken == securityToken);

            userToConfirm.IsConfirmed = true;

            _unitOfWork.Save();
        }


    }

}
