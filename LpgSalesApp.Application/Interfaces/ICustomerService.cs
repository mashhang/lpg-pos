using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpgSalesApp.Application.DTOs;

namespace LpgSalesApp.Application.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync();
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CustomerDto customer);
    Task<CustomerDto> UpdateAsync(CustomerDto customer);
    Task<bool> DeleteAsync(int id);

}
