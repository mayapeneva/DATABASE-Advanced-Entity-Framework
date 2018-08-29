namespace CarDealer.App
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using ExportDtos;
    using ImportDtos;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            var config = new MapperConfiguration(cfg => { cfg.AddProfile<CarDealerProfile>(); });
            var mapper = config.CreateMapper();

            ImportData(context, mapper);
            //ExportData(context, mapper);
        }

        private static void ExportData(CarDealerContext context, IMapper mapper)
        {
            GetCarsWithDistance(context, mapper);
            //GetCarsFromMakeFerrari(context, mapper);
            //GetLocalSuppliers(context, mapper);
            //GetCarsWithPartsList(context, mapper);
            //GetTotalSalesByCustomer(context, mapper);
            //GetSalesWithAppliedDiscount(context, mapper);
        }

        private static void GetSalesWithAppliedDiscount(CarDealerContext context, IMapper mapper)
        {
            var sales = context.Sales
                .ProjectTo<SaleWithDiscountDto>(mapper.ConfigurationProvider)
                .ToArray();

            var serializer = new XmlSerializer(typeof(SaleWithDiscountDto[]), new XmlRootAttribute("sales"));

            using (var writer = new StreamWriter("../../../XMLExport/sales-discounts"))
            {
                serializer.Serialize(writer, sales, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetTotalSalesByCustomer(CarDealerContext context, IMapper mapper)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .OrderByDescending(c => c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price)))
                .ThenByDescending(c => c.Sales.Count)
                .ProjectTo<CustomerSalesDto>(mapper.ConfigurationProvider)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CustomerSalesDto[]), new XmlRootAttribute("customers"));

            using (var writer = new StreamWriter("../../../XMLExport/customer-total-sales.xml"))
            {
                serializer.Serialize(writer, customers, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCarsWithPartsList(CarDealerContext context, IMapper mapper)
        {
            var cars = context.Cars.ProjectTo<CarWithPartsListDto>(mapper.ConfigurationProvider).ToArray();

            var serializer = new XmlSerializer(typeof(CarWithPartsListDto[]), new XmlRootAttribute("cars"));

            using (var writer = new StreamWriter("../../../XMLExport/cars-and-parts.xml"))
            {
                serializer.Serialize(writer, cars, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetLocalSuppliers(CarDealerContext context, IMapper mapper)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .OrderBy(s => s.Name)
                .ProjectTo<SupplierLocalDto>(mapper.ConfigurationProvider)
                .ToArray();

            var serializer = new XmlSerializer(typeof(SupplierLocalDto[]), new XmlRootAttribute("suppliers"));

            using (var writer = new StreamWriter("../../../XMLExport/local-suppliers"))
            {
                serializer.Serialize(writer, suppliers, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCarsFromMakeFerrari(CarDealerContext context, IMapper mapper)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Ferrari")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<CarFerrariDto>(mapper.ConfigurationProvider)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CarFerrariDto[]), new XmlRootAttribute("cars"));

            using (var writer = new StreamWriter("../../../XMLExport/ferrari-cars.xml"))
            {
                serializer.Serialize(writer, cars, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCarsWithDistance(CarDealerContext context, IMapper mapper)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .ProjectTo<CarDistanceDto>(mapper.ConfigurationProvider)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CarDistanceDto[]), new XmlRootAttribute("cars"));

            using (var writer = new StreamWriter("../../../XMLExport/cars.xml"))
            {
                serializer.Serialize(writer, cars, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void ImportData(CarDealerContext context, IMapper mapper)
        {
            ImportSuppliers(context, mapper);
            //ImportParts(context, mapper);
            //ImportCars(context, mapper);
            //ImportPartCars(context);
            //ImportCustomers(context, mapper);
            //ImportSales(context);
        }

        private static void ImportSales(CarDealerContext context)
        {
            var sales = new List<Sale>();
            var discountRange = new List<int> { 0, 5, 10, 15, 20, 30, 40, 50 };

            var cars = context.Cars;
            foreach (var car in cars)
            {
                sales.Add(new Sale
                {
                    CarId = car.Id,
                    CustomerId = new Random().Next(1, 31),
                    Discount = discountRange[new Random().Next(0, discountRange.Count)]
                });
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();
        }

        private static void ImportCustomers(CarDealerContext context, IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XMLImport/customers.xml");

            var serializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("customers"));
            var dtoCustomers = (CustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            var customers = new List<Customer>();
            foreach (var dtoCustomer in dtoCustomers)
            {
                customers.Add(mapper.Map<Customer>(dtoCustomer));
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        private static void ImportPartCars(CarDealerContext context)
        {
            var carIds = context.Cars.Select(c => c.Id);

            var partCars = new List<PartCar>();
            foreach (var carId in carIds)
            {
                var partIds = new List<int>();
                for (int i = 1; i < 132; i++)
                {
                    partIds.Add(i);
                }

                var partsCount = new Random().Next(10, 21);
                for (int j = 0; j < partsCount; j++)
                {
                    var partCar = new PartCar
                    {
                        CarId = carId,
                        PartId = partIds[new Random().Next(0, partIds.Count - 1)]
                    };

                    partIds.Remove(partCar.PartId);
                    partCars.Add(partCar);
                }
            }

            context.PartCars.AddRange(partCars);
            context.SaveChanges();
        }

        private static void ImportCars(CarDealerContext context, IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XMLImport/cars.xml");

            var serizlizer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("cars"));
            var dtoCars = (CarDto[])serizlizer.Deserialize(new StringReader(xmlString));

            var cars = new List<Car>();
            foreach (var dtoCar in dtoCars)
            {
                cars.Add(mapper.Map<Car>(dtoCar));
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();
        }

        private static void ImportParts(CarDealerContext context, IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XMLImport/parts.xml");

            var serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("parts"));
            var dtoParts = (PartDto[])serializer.Deserialize(new StringReader(xmlString));

            var parts = new List<Part>();
            foreach (var dtoPart in dtoParts)
            {
                var part = mapper.Map<Part>(dtoPart);
                part.SupplierId = new Random().Next(1, 32);
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
        }

        private static void ImportSuppliers(CarDealerContext context, IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XMLImport/suppliers.xml");

            var serializer = new XmlSerializer(typeof(SupplierDto[]), new XmlRootAttribute("suppliers"));
            var dtoSuppliers = (SupplierDto[])serializer.Deserialize(new StringReader(xmlString));

            var suppliers = new List<Supplier>();
            foreach (var dtoSupplier in dtoSuppliers)
            {
                suppliers.Add(mapper.Map<Supplier>(dtoSupplier));
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
        }

        //private static bool IsValid(object obj)
        //{
        //    var validationContext = new DataAnotations.ValidationContext(obj);
        //    var validationResult = new List<DataAnotations.ValidationResult>();

        //    return DataAnotations.Validator.TryValidateObject(obj, validationContext, validationResult, true);
        //}
    }
}