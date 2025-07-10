using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LpgSalesApp.Application.DTOs;

namespace LpgSalesApp.Application.Validators;

public class CustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100);

        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Contact number is required")
            .MinimumLength(10)
            .MaximumLength(15);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required");
    }
}
