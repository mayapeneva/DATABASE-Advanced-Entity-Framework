namespace ProductShop.App
{
    using AutoMapper;
    using ExportDtos;
    using Models;
    using System.Linq;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ProductInRangeDto>()
                .ForMember(dto => dto.SellerName, dest => dest.MapFrom(p => p.Seller.FirstName + " " + p.Seller.LastName));

            this.CreateMap<User, UserSoldProductsDto>()
                .ForMember(dto => dto.ProductsSold, dest => dest.MapFrom(u => u.ProductsSold.Where(p => p.BuyerId != null)));

            this.CreateMap<Product, ProductSoldDto>()
                .ForMember(dto => dto.BuyerFirstName, dest => dest.MapFrom(p => p.Buyer.FirstName))
                .ForMember(dto => dto.BuyerLastName, dest => dest.MapFrom(p => p.Buyer.LastName));

            this.CreateMap<Category, CategoryByProductsDto>()
                .ForMember(dto => dto.ProductsCount, dest => dest.MapFrom(c => c.CategoryProducts.Count))
                .ForMember(dto => dto.AveragePrice,
                    dest => dest.MapFrom(c => c.CategoryProducts.Sum(cp => cp.Product.Price) / c.CategoryProducts.Count))
                .ForMember(dto => dto.TotalRevenue,
                    dest => dest.MapFrom(c => c.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}