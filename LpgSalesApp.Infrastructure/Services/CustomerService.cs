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

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        return await _context.Customers
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                ContactNumber = c.ContactNumber,
                Address = c.Address
            })
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        return customer == null ? null : new CustomerDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            ContactNumber = customer.ContactNumber,
            Address = customer.Address
        };
    }

    public async Task<CustomerDto> CreateAsync(CustomerDto dto)
    {
        var entity = new Customer
        {
            FullName = dto.FullName,
            ContactNumber = dto.ContactNumber,
            Address = dto.Address
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        dto.Id = entity.Id;
        return dto;
    }

    public async Task<CustomerDto> UpdateAsync(CustomerDto dto)
    {
        var entity = await _context.Customers.FindAsync(dto.Id);
        if (entity == null) throw new Exception("Customer not found");

        entity.FullName = dto.FullName;
        entity.ContactNumber = dto.ContactNumber;
        entity.Address = dto.Address;

        await _context.SaveChangesAsync();
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Customers.FindAsync(id);
        if (entity == null) return false;

        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
