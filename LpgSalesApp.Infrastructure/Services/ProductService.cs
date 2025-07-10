using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.Domain.Entities;
using LpgSalesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LpgSalesApp.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product == null ? null : new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto)
    {
        var entity = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        dto.Id = entity.Id;
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(ProductDto dto)
    {
        var entity = await _context.Products.FindAsync(dto.Id);
        if (entity == null) throw new Exception("Product not found");

        entity.Name = dto.Name;
        entity.Price = dto.Price;
        entity.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity == null) return false;

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
