using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
