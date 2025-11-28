using School.Application.Dtos.Auth;

namespace School.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(LoginRequest request);
        Task<TokenResponse> RegisterAsync(RegisterRequest request);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
