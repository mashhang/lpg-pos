using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Application.Interfaces;

public interface IUserService
{
    UserDto? Authenticate(string username, string password);
    Task<AppUser> CreateUserAsync(string username, string password, string role);
    Task<UserDto?> AuthenticateAsync(string username, string password);
    Task<List<UserDto>> GetAllUsersAsync();
    Task DeleteUserAsync(int userId);
    Task UpdateUserAsync(int userId, string username, string role, string? password = null);

}