using LpgSalesApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductDto product);
    Task<ProductDto> UpdateAsync(ProductDto product);
    Task<bool> DeleteAsync(int id);
}
