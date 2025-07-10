using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Domain.Entities;

public class InventoryLog
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int QuantityChanged { get; set; } // e.g. +10 for restock, -1 for sale
    public string Action { get; set; } = ""; // "Restock", "Sale", "Damage"
    public DateTime Date { get; set; } = DateTime.Now;

    public Product? Product { get; set; }
}
