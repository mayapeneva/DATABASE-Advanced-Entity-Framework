namespace CarDealer.App
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Dtos;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var config = new MapperConfiguration(c => { c.AddProfile<CarDealerProfile>(); });
            var mapper = config.CreateMapper();

            var context = new CarDealerContext();

            //ImportData(context);
            ExportData(context, mapper);
        }

        private static void ExportData(CarDealerContext context, IMapper mapper)
        {
            //GetOrderedCustomers(context, mapper);
            //GetCarsFromMakeToyota(context, mapper);
            //GetLocalSuppliers(context, mapper);
            //GetCarsWithTheirListOfParts(context, mapper);
            //GetTotalSalesByCustomer(context, mapper);
            GetSalesWithAppliedDiscount(context, mapper);
        }

        private static void GetSalesWithAppliedDiscount(CarDealerContext context, IMapper mapper)
        {
            var sales = context.Sales
                .ProjectTo<SaleWithDiscountDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/sales-discounts.json", json);
        }

        private static void GetTotalSalesByCustomer(CarDealerContext context, IMapper mapper)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .OrderByDescending(c => c.Sales.Sum(s => s.Car.Parts.Sum(p => p.Part.Price)))
                .ThenByDescending(c => c.Sales.Count)
                .ProjectTo<CustomerWithSalesDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/customers-total-sales.json", json);
        }

        private static void GetCarsWithTheirListOfParts(CarDealerContext context, IMapper mapper)
        {
            var cars = context.Cars
                .ProjectTo<CarWithPartsDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/cars-and-parts.json", json);
        }

        private static void GetLocalSuppliers(CarDealerContext context, IMapper mapper)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .OrderBy(s => s.Name)
                .ProjectTo<LocalSupplierDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/local-suppliers.json", json);
        }

        private static void GetCarsFromMakeToyota(CarDealerContext context, IMapper mapper)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ToyotaCarDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/toyota-cars.json", json);
        }

        private static void GetOrderedCustomers(CarDealerContext context, IMapper mapper)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ProjectTo<OrderedCustomerDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/ordered-customers.json", json);
        }

        private static void ImportData(CarDealerContext context)
        {
            ImportSuppliers(context);
            ImportParts(context);
            ImportCars(context);
            ImportCustomers(context);
            ImportSales(context);
        }

        private static void ImportSales(CarDealerContext context)
        {
            var sales = new List<Sale>();
            var discounts = new List<int> { 0, 5, 10, 15, 20, 30, 40, 50 };

            var cars = context.Cars;
            foreach (var car in cars)
            {
                var sale = new Sale()
                {
                    CarId = car.Id,
                    CustomerId = new Random().Next(1, 31),
                    Discount = discounts[new Random().Next(0, discounts.Count)]
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();
        }

        private static void ImportCustomers(CarDealerContext context)
        {
            var objCustomers =
                JsonConvert.DeserializeObject<Customer[]>(File.ReadAllText("../../../ImportFiles/customers.json"), new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.IsoDateFormat });

            var customers = new List<Customer>();
            foreach (var customer in objCustomers)
            {
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        private static void ImportCars(CarDealerContext context)
        {
            var objCars = JsonConvert.DeserializeObject<Car[]>(File.ReadAllText("../../../ImportFiles/cars.json"));

            var cars = new List<Car>();
            foreach (var car in objCars)
            {
                var partsNumber = new Random().Next(10, 21);
                for (int i = 0; i < partsNumber; i++)
                {
                    var partId = new Random().Next(1, 132);
                    while (car.Parts.Any(p => p.PartId == partId))
                    {
                        partId = new Random().Next(1, 132);
                    }

                    var partCar = new PartCar()
                    {
                        CarId = car.Id,
                        PartId = partId
                    };

                    car.Parts.Add(partCar);
                }

                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();
        }

        private static void ImportParts(CarDealerContext context)
        {
            var objParts = JsonConvert.DeserializeObject<Part[]>(File.ReadAllText("../../../ImportFiles/parts.json"));

            var parts = new List<Part>();
            foreach (var part in objParts)
            {
                part.SupplierId = new Random().Next(1, 32);
                parts.Add(part);
            }

            context.AddRange(parts);
            context.SaveChanges();
        }

        private static void ImportSuppliers(CarDealerContext context)
        {
            var objSuppliers = JsonConvert.DeserializeObject<Supplier[]>(File.ReadAllText("../../../ImportFiles/suppliers.json"));

            var suppliers = new List<Supplier>();
            foreach (var supplier in objSuppliers)
            {
                suppliers.Add(supplier);
            }

            context.AddRange(suppliers);
            context.SaveChanges();
        }
    }
}