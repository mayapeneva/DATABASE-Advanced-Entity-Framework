namespace BookShop
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using BookShop.Data;
    using BookShop.Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Primitives;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                //DbInitializer.ResetDatabase(db);

                //var command = Console.ReadLine();
                //Console.WriteLine(GetBooksByAgeRestriction(db, command));

                //Console.WriteLine(GetGoldenBooks(db));

                //Console.WriteLine(GetBooksByPrice(db));

                //var year = int.Parse(Console.ReadLine());
                //Console.WriteLine(GetBooksNotRealeasedIn(db, year));

                //var input = Console.ReadLine();
                //Console.WriteLine(GetBooksByCategory(db, input));

                //var date = Console.ReadLine();
                //Console.WriteLine(GetBooksReleasedBefore(db, date));

                //var input = Console.ReadLine();
                //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

                //var input = Console.ReadLine();
                //Console.WriteLine(GetBookTitlesContaining(db, input));

                //var input = Console.ReadLine();
                //Console.WriteLine(GetBooksByAuthor(db, input));

                //var lengthCheck = int.Parse(Console.ReadLine());
                //Console.WriteLine(CountBooks(db, lengthCheck));

                //Console.WriteLine(CountCopiesByAuthor(db));

                //Console.WriteLine(GetTotalProfitByCategory(db));

                //Console.WriteLine(GetMostRecentBooks(db));

                //IncreasePrices(db);

                Console.WriteLine(RemoveBooks(db));
            }
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200);
            var booksCount = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return booksCount;
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);
            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks.OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Select(cb => cb.Book.Title + " (" + cb.Book.ReleaseDate.Value.Year + ")").Take(3)
                })
                .OrderBy(c => c.CategoryName)
                .ToArray();

            var result = new StringBuilder();
            foreach (var category in categories)
            {
                result.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.Books)
                {
                    result.AppendLine(book);
                }
            }

            return result.ToString().Trim();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.CategoryName)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => b.CategoryName + " $" + b.Profit));
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var booksByAuthor = context.Authors
                .Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    BooksCount = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.BooksCount)
                .ToArray();

            return string.Join(Environment.NewLine, booksByAuthor.Select(b => b.AuthorName + " - " + b.BooksCount));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Count(b => b.Title.Length > lengthCheck);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books.Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower())).OrderBy(b => b.BookId).Select(b =>
                new
                {
                    PrintName = $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})"
                }).ToArray();

            return string.Join(Environment.NewLine, books.Select(b => b.PrintName));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books.Where(b => b.Title.ToLower().Contains(input.ToLower())).OrderBy(b => b.Title)
                .Select(b => b.Title).ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName
                })
                .ToArray();

            return string.Join(Environment.NewLine, authors.Select(a => a.Name));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var input = date.Split('-').Select(int.Parse).ToArray();
            var targetDate = new DateTime(input[2], input[1], input[0]);

            var books = context.Books.Where(b => b.ReleaseDate < targetDate).OrderByDescending(b => b.ReleaseDate).Select(b => new
            {
                PrintName = $"{b.Title} - {b.EditionType} - ${b.Price:F2}"
            }).ToArray();

            return string.Join(Environment.NewLine, books.Select(b => b.PrintName));
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books.Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title).OrderBy(b => b).ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year != year).OrderBy(b => b.BookId)
                .Select(b => b.Title);

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    PrintName = $"{b.Title} - ${b.Price:F2}"
                })
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => b.PrintName));
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000).OrderBy(b => b.BookId).Select(b => b.Title).ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRest = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(command));
            var books = context.Books.Where(b => b.AgeRestriction == ageRest).Select(b => b.Title).OrderBy(t => t).ToArray();

            return string.Join(Environment.NewLine, books);
        }
    }
}