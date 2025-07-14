using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }
    public string? Remarks { get; set; }
    public ICollection<TransactionItem> Items { get; set; } = new List<TransactionItem>();

    public int CreatedByUserId { get; set; }

    public User CreatedByUser { get; set; }

    public int? ModifiedByUserId { get; set; }

    public User? ModifiedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? ModifiedAt { get; set; }
}
