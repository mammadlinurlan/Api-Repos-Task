using Api.Shop.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Shop.Apps.AdminApi.DTOs.ProductDtos
{
    public class ProductPostDto
    {
        public string Name { get; set; }

        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public IFormFile ImageFile { get; set; }
        public int CategoryId { get; set; }

    }
    public class ProductPostDtoValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostDtoValidator()
        {
            RuleFor(x => x.CategoryId).NotNull().GreaterThanOrEqualTo(1);


            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("Uzunluq max 50 ola biler!")
                .NotEmpty().WithMessage("Name mecburidir!");

          
            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0).WithMessage("CostPrice 0-dan asagi ola bilmez!")
                .NotEmpty().WithMessage("CostPrice mecburidir!");

           

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("SalePrice 0-dan asagi ola bilmez!")
                .NotEmpty().WithMessage("SalePrice mecburidir!");


            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.CostPrice > x.SalePrice)
                    context.AddFailure("CostPrice", "CostPrice SalePrice-dan boyuk ola bilmez");
            });

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.ImageFile == null)
                {
                    context.AddFailure("ImageFile", "Image cannot be null!");
                }
                else if (!x.ImageFile.IsImage())
                {
                    context.AddFailure("Please select an image file");
                }
                else if (!x.ImageFile.IsSizeOkay(2))
                {
                    context.AddFailure("Image size must be less than 2mb");
                }

            });
        }
    }
}
