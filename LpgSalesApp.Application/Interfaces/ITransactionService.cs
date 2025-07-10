using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpgSalesApp.Application.DTOs;

namespace LpgSalesApp.Application.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetAllAsync();
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto> CreateAsync(TransactionDto transaction);
    Task<List<TransactionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
