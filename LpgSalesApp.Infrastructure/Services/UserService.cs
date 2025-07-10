using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
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


    public UserDto? Authenticate(string username, string password)
    {
        var passwordHash = ComputeSha256Hash(password);

        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

        return user == null ? null : new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
        return Convert.ToHexString(bytes).ToLower();
    }

}
