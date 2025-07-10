using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Application.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
