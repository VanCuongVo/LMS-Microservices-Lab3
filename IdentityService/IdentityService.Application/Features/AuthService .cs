using IdentityService.Application.DTOs.Request;
using IdentityService.Application.DTOs.Response;
using IdentityService.Application.Interfaces;
using IdentityService.Application.IServices;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Enum;
using IdentityService.Domain.IUnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Application.Features
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IJwtService _jwtService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IConfiguration _configuration;



        public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, IUnitOfWork unitOfWork,
        IConfiguration configuration
        )
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user =
             await _userManager.FindByEmailAsync(
                 request.Email);

            if (user == null)
                return null;

            var valid =
                await _userManager.CheckPasswordAsync(
                    user,
                    request.Password);

            if (!valid)
                return null;

            var token =
                await _jwtService.GenerateTokenAsync(user);

            var refreshToken = _jwtService.GenerateRefreshToken();
            await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_configuration.GetSection("Jwt")["RefreshTokenExpirationDays"] ?? "7")),
            });
            await _unitOfWork.SaveChangesAsync();


            return new LoginResponse
            {
                AccessToken = token,
                Email = user.Email!,
                RefreshToken = refreshToken,
                ExpiresIn = (int)TimeSpan.FromMinutes(double.Parse(_configuration.GetSection("Jwt")["AccessTokenExpirationMinutes"] ?? "60")).TotalSeconds
            };
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);
            if (existingToken == null)
            {
                throw new Exception("Refresh token not found");
            }
            if (existingToken.IsRevoked)
            {
                throw new Exception("Refresh token revoked");
            }
            if (existingToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new Exception("Refresh token expired");
            }
            var user = existingToken.User;
            var accessToken = await _jwtService.GenerateTokenAsync(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            existingToken.Token = newRefreshToken;
            existingToken.ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_configuration.GetSection("Jwt")["RefreshTokenExpirationDays"] ?? "7"));
            _unitOfWork.RefreshTokens.Update(existingToken);
            await _unitOfWork.SaveChangesAsync();

            return new RefreshTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = (int)TimeSpan.FromMinutes(double.Parse(_configuration.GetSection("Jwt")["AccessTokenExpirationMinutes"] ?? "60")).TotalSeconds

            };
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);


            if (!result.Succeeded)
            {
                throw new Exception(
            string.Join(" | ",
                result.Errors.Select(x => x.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, RoleEnum.Student.ToString());

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    string.Join(" | ",
                        roleResult.Errors.Select(x => x.Description)));
            }

            return true;
        }
    }
}
