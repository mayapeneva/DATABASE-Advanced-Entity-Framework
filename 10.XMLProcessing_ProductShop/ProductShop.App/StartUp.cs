using System;

namespace ProductShop.App
{
    using System.Collections.Generic;
    using DataAnotations = System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using DTOs;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<ProductShopProfile>(); });
            var mapper = config.CreateMapper();

            var context = new ProductShopContext();

            ImportData(mapper, context);

            //ExportData(mapper, context);
        }

        private static void ExportData(IMapper mapper, ProductShopContext context)
        {
            ProductsInRange(mapper, context);
            SoldProducts(mapper, context);
            CategoriesByProductsCount(mapper, context);
            UsersAndProducts(context);
        }

        private static void UsersAndProducts(ProductShopContext context)
        {
            var users = new UsersProductsDto
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
                            Age = u.Age.ToString(),
                            SoldProducts = new Sold_ProductsDto
                            {
                                ProductsCount = u.ProductsSold.Count,
                                Products = u.ProductsSold.Select(pr => new Sold_ProductDto
                                {
                                    Name = pr.Name,
                                    Price = pr.Price
                                }).ToList()
                            }
                        }).ToList()
            };

            var serializer = new XmlSerializer(typeof(UsersProductsDto), new XmlRootAttribute("users"));

            using (var writer = new StreamWriter("../../../OutXMLFiles/users-and-products"))
            {
                serializer.Serialize(writer, users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void CategoriesByProductsCount(IMapper mapper, ProductShopContext context)
        {
            var serializer = new XmlSerializer(typeof(CategoryByProductCountDto[]), new XmlRootAttribute("categories"));

            var categories = context.Categories.OrderByDescending(c => c.CategoryProducts.Count)
                .ProjectTo<CategoryByProductCountDto>(mapper.ConfigurationProvider).ToArray();

            using (var writer = new StreamWriter("../../../OutXMLFiles/categories-by-products"))
            {
                serializer.Serialize(writer, categories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void SoldProducts(IMapper mapper, ProductShopContext context)
        {
            var serializer = new XmlSerializer(typeof(UserSoldProductsDto[]), new XmlRootAttribute("users"));

            var users = context.Users.Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .ProjectTo<UserSoldProductsDto>(mapper.ConfigurationProvider).ToArray();
            using (var writer = new StreamWriter("../../../OutXMLFiles/users-sold-products.xml"))
            {
                serializer.Serialize(writer, users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void ProductsInRange(IMapper mapper, ProductShopContext context)
        {
            var serializer = new XmlSerializer(typeof(ProductInRangeDto[]), new XmlRootAttribute("products"));

            var products = context.Products.Where(p => p.Price >= 1000 && p.Price <= 2000 & p.BuyerId != null)
                .OrderBy(p => p.Price).ProjectTo<ProductInRangeDto>(mapper.ConfigurationProvider).ToArray();

            using (var writer = new StreamWriter("../../../OutXMLFiles/products-in-rage.xml"))
            {
                serializer.Serialize(writer, products, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void ImportData(IMapper mapper, ProductShopContext context)
        {
            InsertUsersIntoDatabase(mapper, context);
            InserProductsIntoDatabase(mapper, context);
            InsertCategoriesIntoDatabase(mapper, context);
            InsertCategoryProductsIntoDatabase(context);
        }

        private static void InsertCategoryProductsIntoDatabase(ProductShopContext context)
        {
            var categoryProducts = new List<CategoryProduct>();
            var products = context.Products;
            foreach (var product in products)
            {
                categoryProducts.Add(new CategoryProduct()
                {
                    ProductId = product.Id,
                    CategoryId = new Random().Next(1, 12)
                });
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void InsertCategoriesIntoDatabase(IMapper mapper, ProductShopContext context)
        {
            var xmlString = File.ReadAllText("../../../XMLFiles/categories.xml");

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("categories"));
            var deserializedCategories = (CategoryDto[])serializer.Deserialize(new StringReader(xmlString));

            var categories = new List<Category>();
            foreach (var dCategory in deserializedCategories)
            {
                if (!IsValid(dCategory))
                {
                    continue;
                }

                var category = mapper.Map<Category>(dCategory);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void InserProductsIntoDatabase(IMapper mapper, ProductShopContext context)
        {
            var xmlString = File.ReadAllText("../../../XMLFiles/products.xml");

            var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));
            var deserializedProducts = (ProductDto[])serializer.Deserialize(new StringReader(xmlString));

            var products = new List<Product>();
            var counter = 1;
            foreach (var dProduct in deserializedProducts)
            {
                if (!IsValid(dProduct))
                {
                    continue;
                }

                var product = mapper.Map<Product>(dProduct);

                var buyerId = new Random().Next(1, 29);
                var sellerId = new Random().Next(29, 57);

                product.BuyerId = buyerId;
                product.SellerId = sellerId;

                if (counter == 5)
                {
                    product.BuyerId = null;
                    counter = 1;
                }
                counter++;

                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void InsertUsersIntoDatabase(IMapper mapper, ProductShopContext context)
        {
            var xmlString = File.ReadAllText("../../../XMLFiles/users.xml");

            var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));
            var deserializedUsers = (UserDto[])serializer.Deserialize(new StringReader(xmlString));

            var users = new List<User>();
            foreach (var dUser in deserializedUsers)
            {
                if (!IsValid(dUser))
                {
                    continue;
                }

                var user = mapper.Map<User>(dUser);
                users.Add(user);
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