using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpgSalesApp.Application.DTOs;

namespace LpgSalesApp.Application.Interfaces;

public interface IUserService
{
    UserDto? Authenticate(string username, string password);

}