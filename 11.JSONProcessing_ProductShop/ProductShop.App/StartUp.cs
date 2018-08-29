namespace ProductShop.App
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using ExportDtos;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DataAnotations = System.ComponentModel.DataAnnotations;

    public class StartUp
    {
        public static void Main()
        {
            var config = new MapperConfiguration(c => { c.AddProfile<ProductShopProfile>(); });
            var mapper = config.CreateMapper();

            var context = new ProductShopContext();

            //ImportData(context);
            ExportData(context, mapper);
        }

        private static void ExportData(ProductShopContext context, IMapper mapper)
        {
            //GetProductsInRange(context, mapper);
            //GetSuccessfullySoldProducts(context, mapper);
            GetCategoriesByProductsCount(context, mapper);
            //GetUsersAndProducts(context, mapper);
        }

        private static void GetUsersAndProducts(ProductShopContext context, IMapper mapper)
        {
            var users = new UsersProductsDto()
            {
                UsersCount = context.Users.Count(),
                Users = context.Users
                        .Where(u => u.ProductsSold.Count > 0)
                        .OrderByDescending(u => u.ProductsSold.Count)
                        .ThenBy(u => u.LastName)
                        .Select(u => new UserProductsDto
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Age = u.Age,
                            SoldProducts = new ProductsUsersDto
                            {
                                Count = u.ProductsSold.Count,
                                Products = u.ProductsSold.Select(p => new ProductUsersDto
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                }).ToArray()
                            }
                        }).ToArray()
            };

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/users-and-products.json", json);
        }

        private static void GetCategoriesByProductsCount(ProductShopContext context, IMapper mapper)
        {
            var category = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .ProjectTo<CategoryByProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(category, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/categories-by-products.json", json);
        }

        private static void GetSuccessfullySoldProducts(ProductShopContext context, IMapper mapper)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<UserSoldProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/users-sold-products.json", json);
        }

        private static void GetProductsInRange(ProductShopContext context, IMapper mapper)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText("../../../ExportFiles/products-in-range.json", json);
        }

        private static void ImportData(ProductShopContext context)
        {
            ImportUsers(context);
            ImportProducts(context);
            ImportCategories(context);
            ImportCategoryProducts(context);
        }

        private static void ImportCategoryProducts(ProductShopContext context)
        {
            var products = context.Products;
            var categoryProducts = new List<CategoryProduct>();
            foreach (var product in products)
            {
                var category = new CategoryProduct()
                {
                    ProductId = product.Id,
                    CategoryId = new Random().Next(1, 12)
                };

                categoryProducts.Add(category);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void ImportCategories(ProductShopContext context)
        {
            var objCategories = JsonConvert.DeserializeObject<Category[]>(File.ReadAllText("../../../ImportFiles/categories.json"));

            var categories = new List<Category>();
            foreach (var category in objCategories)
            {
                if (IsValid(category))
                {
                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void ImportProducts(ProductShopContext context)
        {
            var objProaducts =
                JsonConvert.DeserializeObject<Product[]>(File.ReadAllText("../../../ImportFiles/products.json"));

            var counter = 1;
            var products = new List<Product>();
            foreach (var product in objProaducts)
            {
                if (IsValid(product))
                {
                    product.BuyerId = new Random().Next(1, 29);
                    product.SellerId = new Random().Next(29, 57);
                    if (counter == 5)
                    {
                        product.BuyerId = null;
                        counter = 1;
                    }

                    counter++;
                    products.Add(product);
                }
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void ImportUsers(ProductShopContext context)
        {
            var objUsers = JsonConvert.DeserializeObject<User[]>(File.ReadAllText("../../../ImportFiles/users.json"));

            var users = new List<User>();
            foreach (var user in objUsers)
            {
                if (IsValid(user))
                {
                    users.Add(user);
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new DataAnotations.ValidationContext(obj);
            var validationResult = new List<DataAnotations.ValidationResult>();

            return DataAnotations.Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}