using Api.Shop.Apps.AdminApi.DTOs.ProductDtos;
using AutoMapper;
using Shop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Shop.Apps.AdminApi.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CategoryInProductGetDto>();
            CreateMap<Category, CategoryInProductListItemDto>();
            CreateMap<Product, ProductGetDto>().ForMember(dest => dest.Profit, map => map.MapFrom(src => src.SalePrice - src.CostPrice));

        }
    }
}
