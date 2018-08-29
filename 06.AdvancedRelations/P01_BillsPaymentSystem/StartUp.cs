namespace P01_BillsPaymentSystem
{
    using Data;
    using Data.Models.Models;
    using Microsoft.EntityFrameworkCore;
    using Seeder;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Seed.Insert(context);

                //UserDetails(context);

                PayBills(context);
            }
        }

        private static void PayBills(BillsPaymentSystemContext context)
        {
            var user = GetUser(context);

            Console.Write("Please, enter the amount that needs to be paid: ");
            var amount = decimal.Parse(Console.ReadLine());

            var accounts = user.PaymentMethods.Where(m => m.BankAccount != null).Select(m => m.BankAccount).OrderBy(a => a.BankAccountId).ToArray();
            var cards = user.PaymentMethods.Where(m => m.CreditCard != null).Select(m => m.CreditCard)
                .OrderBy(c => c.CreditCardId).ToArray();

            var userTotalMoney = accounts.Sum(a => a.Balance) + cards.Sum(c => c.LimitLeft);
            if (userTotalMoney < amount)
            {
                Console.WriteLine("Insufficient funds!");
                Environment.Exit(0);
            }

            if (accounts.Any())
            {
                foreach (var account in accounts)
                {
                    if (account.Balance >= amount)
                    {
                        account.Withdraw(amount);
                        context.SaveChanges();
                        Environment.Exit(0);
                    }
                    else
                    {
                        account.Withdraw(account.Balance);
                        amount -= account.Balance;
                    }
                }
            }

            if (cards.Any())
            {
                foreach (var card in cards)
                {
                    if (card.LimitLeft >= amount)
                    {
                        card.Withdraw(amount);
                        context.SaveChanges();
                        Environment.Exit(0);
                    }
                    else
                    {
                        card.Withdraw(card.LimitLeft);
                        amount -= card.LimitLeft;
                    }
                }
            }
        }

        private static void UserDetails(BillsPaymentSystemContext context)
        {
            var user = GetUser(context);

            Console.WriteLine($"User: {user.FirstName} {user.LastName}");

            var accounts = user.PaymentMethods.Select(m => m.BankAccount).ToArray();
            if (accounts.Any())
            {
                Console.WriteLine("Bank Accounts:");
                foreach (var account in accounts)
                {
                    Console.WriteLine($"-- ID: {account.BankAccountId}");
                    Console.WriteLine($"--- Balance: {account.Balance:F2}");
                    Console.WriteLine($"--- Bank: {account.BankName}");
                    Console.WriteLine($"--- SWIFT: {account.SwiftCode}");
                }
            }
            else
            {
                Console.WriteLine($"{user.FirstName} {user.LastName} has no bank accounts.");
            }

            var cards = user.PaymentMethods.Select(m => m.CreditCard).ToArray();
            if (cards.Any())
            {
                Console.WriteLine("Credit Cards:");
                foreach (var card in cards)
                {
                    Console.WriteLine($"-- ID: {card.CreditCardId}");
                    Console.WriteLine($"--- Limit: {card.Limit:F2}");
                    Console.WriteLine($"--- Money Owed: {card.MoneyOwed:F2}");
                    Console.WriteLine($"--- Limit Left: {card.LimitLeft:F2}");
                    Console.WriteLine($"Exparation Date: {card.ExpirationDate.Year}/{card.ExpirationDate.Month}");
                }
            }
            else
            {
                Console.WriteLine($"{user.FirstName} {user.LastName} has no credit cards.");
            }
        }

        private static User GetUser(BillsPaymentSystemContext context)
        {
            Console.Write("Please, enter userId: ");
            var userId = int.Parse(Console.ReadLine());

            var user = context.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(m => m.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(m => m.CreditCard)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                Environment.Exit(0);
            }

            return user;
        }
    }
}