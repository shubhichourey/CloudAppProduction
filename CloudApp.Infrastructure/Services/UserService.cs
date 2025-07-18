using CloudApp.Application.DTOs;
using CloudApp.Application.Interfaces;
using CloudApp.Domain.Entities;
using CloudApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System;

namespace CloudApp.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public UserService(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return (false, "Passwords do not match");

        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            return (false, "Email already registered");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(user.Email, "Welcome to the App", "Thank you for signing up!");

        return (true, "User registered successfully");
    }

    public async Task<(bool Success, string Message)> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            return (false, "User is not registered, SignUp!");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return (false, "Invalid username or password");

        await _emailService.SendEmailAsync(user.Email, "Welcome back!", "You have successfully logged in.");

        return (true, "Login successful");
    }
}