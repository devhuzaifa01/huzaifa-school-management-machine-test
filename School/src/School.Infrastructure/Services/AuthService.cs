using Microsoft.Extensions.Options;
using School.Application.Common.Errors;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos.Auth;
using School.Domain.Entities;
using School.Infrastructure.Identity;

namespace School.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        // We are using an in memory store for RefreshTokens until we implement it in DB
        private static readonly Dictionary<string, RefreshToken> _refreshTokens = new();

        public AuthService(IUserRepository userRepository, JwtTokenService jwtTokenService, IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
                throw new UnauthorizedException("Invalid email or password");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid email or password");

            var claims = _jwtTokenService.BuildAuthClaims(user);
            var token = _jwtTokenService.GenerateJwtToken(claims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            _refreshTokens[refreshToken] = new RefreshToken
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            };

            return new TokenResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt
            };
        }

        public async Task<TokenResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new BusinessException("User with this email already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword,
                Role = request.Role,
                CreatedDate = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            // Generate JWT token
            var claims = _jwtTokenService.BuildAuthClaims(user);
            var token = _jwtTokenService.GenerateJwtToken(claims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            // Store refresh token
            _refreshTokens[refreshToken] = new RefreshToken
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            };

            return new TokenResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt
            };
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken) || !_refreshTokens.TryGetValue(request.RefreshToken, out var tokenInfo))
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            if (tokenInfo.ExpiresAt < DateTime.UtcNow)
            {
                _refreshTokens.Remove(request.RefreshToken);
                throw new UnauthorizedException("Refresh token has expired");
            }

            var user = new User
            {
                Id = tokenInfo.UserId,
                Email = tokenInfo.Email,
                Name = tokenInfo.Name,
                Role = tokenInfo.Role
            };

            var claims = _jwtTokenService.BuildAuthClaims(user);
            var newToken = _jwtTokenService.GenerateJwtToken(claims);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            _refreshTokens.Remove(request.RefreshToken);
            _refreshTokens[newRefreshToken] = new RefreshToken
            {
                UserId = tokenInfo.UserId,
                Email = tokenInfo.Email,
                Name = tokenInfo.Name,
                Role = tokenInfo.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            };

            return new TokenResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = expiresAt
            };
        }
    }
}
