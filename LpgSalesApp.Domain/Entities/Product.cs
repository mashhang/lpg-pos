using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., 11kg LPG Tank
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();
}
