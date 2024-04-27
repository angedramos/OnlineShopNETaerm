using FluentValidation;
using OnlineShopNET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopNET.Application.Validations
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.product_name).NotEmpty().MaximumLength(100);
            RuleFor(product => product.product_description).NotEmpty().MaximumLength(200);
            RuleFor(product => product.product_quantity).GreaterThanOrEqualTo(1);
            RuleFor(product => product.product_price).GreaterThanOrEqualTo(1);
            RuleFor(product => product.categoryId).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
