using LpgSalesApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.UI.Services;

public class CurrentUserService : ICurrentUserService
{
    public int? UserId => App.CurrentUser?.Id;
}
