namespace CarDealer.App
{
    using AutoMapper;
    using Dtos;
    using Models;
    using System.Linq;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<Customer, OrderedCustomerDto>()
                .ForMember(dto => dto.Sales, dest => dest.Ignore());

            this.CreateMap<Car, ToyotaCarDto>();

            this.CreateMap<Supplier, LocalSupplierDto>();

            this.CreateMap<Car, CarWithPartsDto>()
                .ForMember(dto => dto.Parts, dest => dest.MapFrom(c => c.Parts.Select(p => p.Part)));
            this.CreateMap<Part, PartForCarDto>();

            this.CreateMap<Customer, CustomerWithSalesDto>()
                .ForMember(dto => dto.BoughtCars, dest => dest.MapFrom(c => c.Sales.Count))
                .ForMember(dto => dto.SpentMoney,
                    dest => dest.MapFrom(c => c.Sales.Sum(s => s.Car.Parts.Sum(p => p.Part.Price))));

            this.CreateMap<Sale, SaleWithDiscountDto>()
                .ForMember(dto => dto.Car, dest => dest.MapFrom(s => s.Car))
                .ForMember(dto => dto.CustomerName, dest => dest.MapFrom(s => s.Customer.Name))
                .ForMember(dto => dto.Price, dest => dest.MapFrom(s => s.Car.Parts.Sum(p => p.Part.Price)))
                .ForMember(dto => dto.PriceWithDiscount, dest => dest.MapFrom(s => s.Car.Parts.Sum(p => p.Part.Price) - (s.Car.Parts.Sum(p => p.Part.Price) * s.Discount / 100)));
            this.CreateMap<Car, CarSaleDto>();
        }
    }
}