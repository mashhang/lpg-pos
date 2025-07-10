using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }
    public ICollection<TransactionItem> Items { get; set; } = new List<TransactionItem>();
}
