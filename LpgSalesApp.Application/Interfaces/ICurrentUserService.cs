﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpgSalesApp.Application.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
}
