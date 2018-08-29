namespace CarDealer.App
{
    using AutoMapper;
    using ExportDtos;
    using ImportDtos;
    using Models;
    using System;
    using System.Linq;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierDto, Supplier>();
            this.CreateMap<PartDto, Part>();
            this.CreateMap<CarDto, Car>();
            this.CreateMap<CustomerDto, Customer>()
                .ForMember(dto => dto.BirthDate, dest => dest.MapFrom(c => Convert.ToDateTime(c.BirthDate)));

            this.CreateMap<Car, CarDistanceDto>();
            this.CreateMap<Car, CarFerrariDto>();
            this.CreateMap<Supplier, SupplierLocalDto>()
                .ForMember(dto => dto.PartsCount, dest => dest.MapFrom(s => s.Parts.Count));
            this.CreateMap<Car, CarWithPartsListDto>()
                .ForMember(dto => dto.PartsList, dest => dest.MapFrom(c => c.PartCars.Select(pc => pc.Part)));
            this.CreateMap<Part, PartListDto>();
            this.CreateMap<Customer, CustomerSalesDto>()
                .ForMember(dto => dto.BoughtCarsCount, dest => dest.MapFrom(c => c.Sales.Count()))
                .ForMember(dto => dto.SpentMoney,
                    dest => dest.MapFrom(c => c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))));
            this.CreateMap<Sale, SaleWithDiscountDto>()
                .ForMember(dto => dto.CustomerName, dest => dest.MapFrom(s => s.Customer.Name))
                .ForMember(dto => dto.Price, dest => dest.MapFrom(s => s.Car.PartCars.Sum(pc => pc.Part.Price)))
                .ForMember(dto => dto.PriceWithDiscount,
                    dest => dest.MapFrom(s => s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100));
        }
    }
}