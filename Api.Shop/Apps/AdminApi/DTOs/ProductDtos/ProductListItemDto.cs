using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Shop.Apps.AdminApi.DTOs.ProductDtos
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal Profit { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }



        public CategoryInProductListItemDto Category { get; set; }
    }
    public class CategoryInProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
