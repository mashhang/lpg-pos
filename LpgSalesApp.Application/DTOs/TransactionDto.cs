using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Application.DTOs;

public class TransactionItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}

public class TransactionDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Remarks { get; set; }
    public string ProductSummary => Items != null && Items.Any()
    ? string.Join(", ", Items.Select(i => $"{i.ProductName} x{i.Quantity}"))
    : string.Empty;
    public int CreatedByUserId { get; set; }
    public string? CreatedBy { get; set; }
    public List<TransactionItemDto> Items { get; set; } = new();
}
