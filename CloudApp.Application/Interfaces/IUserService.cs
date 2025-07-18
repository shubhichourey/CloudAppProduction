using CloudApp.Application.DTOs;

namespace CloudApp.Application.Interfaces;

public interface IUserService
{
    Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, string Message)> LoginAsync(LoginRequest request);
}