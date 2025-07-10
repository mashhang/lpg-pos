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

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransactionDto>> GetAllAsync()
    {
        return await _context.Transactions
            .Include(t => t.Customer)
            .Include(t => t.Items)
                .ThenInclude(i => i.Product)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                CustomerId = t.CustomerId,
                CustomerName = t.Customer.FullName,
                TransactionDate = t.Date,
                TotalAmount = t.TotalAmount,
                Items = t.Items.Select(i => new TransactionItemDto
                {
                    //ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Subtotal = i.Subtotal,
                    // Optionally include product info if needed:
                     UnitPrice = i.Product.Price
                }).ToList()
            }).ToListAsync();
    }


    //public async Task<List<TransactionDto>> GetByDateRangeAsync(DateTime start, DateTime end)
    //{
    //    return await _context.Transactions
    //        .Include(t => t.Customer)
    //        .Include(t => t.Items)
    //        .Where(t => t.Date >= start && t.Date <= end)
    //        .Select(t => new TransactionDto
    //        {
    //            Id = t.Id,
    //            CustomerId = t.CustomerId,
    //            CustomerName = t.Customer.FullName,
    //            TransactionDate = t.Date,
    //            TotalAmount = t.TotalAmount,
    //            Items = t.Items.Select(i => new TransactionItemDto
    //            {
    //                ProductId = i.ProductId,
    //                Quantity = i.Quantity,
    //                Subtotal = i.Subtotal
    //            }).ToList()
    //        }).ToListAsync();
    //}
    public async Task<List<TransactionDto>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        var transactions = await _context.Transactions
            .Include(t => t.Customer)
            .Include(t => t.Items)
            .Where(t => t.Date >= start && t.Date < end.AddDays(1))
            .ToListAsync();

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            CustomerName = t.Customer?.FullName,
            TransactionDate = t.Date,
            TotalAmount = t.TotalAmount,
            Items = t.Items.Select(i => new TransactionItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        }).ToList();
    }



    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        var t = await _context.Transactions
            .Include(t => t.Items)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (t == null) return null;

        return new TransactionDto
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            TransactionDate = t.Date,
            TotalAmount = t.TotalAmount,
            Items = t.Items.Select(i => new TransactionItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        };
    }

    public async Task<TransactionDto> CreateAsync(TransactionDto dto)
    {
        var transaction = new Transaction
        {
            CustomerId = dto.CustomerId,
            Date = DateTime.Now,
            TotalAmount = dto.TotalAmount,
            Items = dto.Items.Select(i => new TransactionItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        };
        _context.Transactions.Add(transaction);

        // Reduce stock
        foreach (var item in dto.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;
            }
        }

        await _context.SaveChangesAsync();
        dto.Id = transaction.Id;
        dto.CustomerName = transaction.Customer?.FullName ?? "";
        return dto;
    }
}
