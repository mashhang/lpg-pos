using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.Domain.Entities;
using LpgSalesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser> CreateUserAsync(string username, string password, string role)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
            throw new InvalidOperationException("Username already exists.");

        var user = new AppUser
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<UserDto?> AuthenticateAsync(string username, string password)
    {
        var passwordHash = HashPassword(password);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);

        return user == null ? null : new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
    }

    public UserDto? Authenticate(string username, string password)
    {
        var passwordHash = HashPassword(password);

        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

        return user == null ? null : new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role
        }).ToList();
    }


    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        if (user.Role == "Admin")
            throw new InvalidOperationException("Cannot delete an Admin user.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(int userId, string username, string role, string? password = null)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        user.Username = username;
        user.Role = role;

        if (!string.IsNullOrWhiteSpace(password))
        {
            // Example: update plaintext, or hash if you're hashing
            user.PasswordHash = HashPassword(password);
        }

        await _context.SaveChangesAsync();
    }


}
