namespace ProductShop.App
{
    using System.Linq;
    using AutoMapper;
    using DTOs;
    using Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDto, User>();
            this.CreateMap<ProductDto, Product>();
            this.CreateMap<CategoryDto, Category>();

            this.CreateMap<Product, ProductInRangeDto>()
                .ForMember(dto => dto.BuyerName, dest => dest.MapFrom(p => p.Buyer.FirstName + ' ' + p.Buyer.LastName));

            this.CreateMap<User, UserSoldProductsDto>()
                .ForMember(dto => dto.SoldProducts, dest => dest.MapFrom(u => u.ProductsSold));
            this.CreateMap<Product, SoldProductDto>();

            this.CreateMap<Category, CategoryByProductCountDto>()
                .ForMember(dto => dto.ProductsCount, dest => dest.MapFrom(c => c.CategoryProducts.Count))
                .ForMember(dto => dto.AveragePrice,
                    dest => dest.MapFrom(c => c.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(dto => dto.TotalRevenue,
                    dest => dest.MapFrom(c => c.CategoryProducts.Sum(cp => cp.Product.Price)));
            this.CreateMap<Product, Sold_ProductsDto>();
        }
    }
}