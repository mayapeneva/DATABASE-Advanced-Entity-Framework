using System;

namespace P01_BillsPaymentSystem.Seeder
{
    using Data;
    using Data.Models.Enums;
    using Data.Models.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public static class Seed
    {
        public static void Insert(BillsPaymentSystemContext context)
        {
            InsertUsers(context);
            InsertCreditCards(context);
            InsertBankAccounts(context);
            InsertPaymentMethods(context);
        }

        private static void InsertUsers(BillsPaymentSystemContext context)
        {
            var users = new List<User>
            {
                new User {FirstName = "Ivan", LastName = "Ivanov", Email = "i.ivanov@gmailcom", Password = "shao411fa"},
                new User {FirstName = "Petya", LastName = "Petrova", Email = "pepita@abv.bg", Password = "flafeaioi5tqw" },
                new User {FirstName = "Andrey", LastName = "Andreev", Email = "andryuto@abv.bg", Password = "mf9909u48hqiu" },
                new User {FirstName = "Krasi", LastName = "Krasimirova", Email = "kykyla@msn.com", Password = "oaoiefnie000" },
                new User {FirstName = "Vurban", LastName = "Vurbanov", Email = "vrubko@gmail.com", Password = "kkgiirn9rr9r0rr0" },
                new User {FirstName = "Slava", LastName = "Slavova", Email = "slavata@gmail.com", Password = "9fe9wbbh38rh" }
            };

            foreach (var user in users)
            {
                if (IsValid(user))
                {
                    context.Users.Add(user);
                }
            }

            context.SaveChanges();
        }

        private static void InsertCreditCards(BillsPaymentSystemContext context)
        {
            var cards = new List<CreditCard>
            {
                new CreditCard{ Limit = 5000, ExpirationDate = new DateTime(2020, 12, 1)},
                new CreditCard{ Limit = 10000, ExpirationDate = new DateTime(2020, 01, 1)},
                new CreditCard{ Limit = 15000, ExpirationDate = new DateTime   (2020, 02, 1)},
                new CreditCard{ Limit = 12000, ExpirationDate = new DateTime   (2020, 05, 1)},
                new CreditCard{ Limit = 7000, ExpirationDate = new DateTime   (2020, 07, 1)},
                new CreditCard{ Limit = 4000, ExpirationDate = new DateTime(2020, 10, 1)}
            };

            foreach (var card in cards)
            {
                card.Deposit(4000);

                if (IsValid(card))
                {
                    context.CreditCards.Add(card);
                }
            }

            context.SaveChanges();
        }

        private static void InsertBankAccounts(BillsPaymentSystemContext context)
        {
            var accounts = new List<BankAccount>
            {
                new BankAccount{BankName = "CCB", SwiftCode = "CECBBGSF"},
                new BankAccount{BankName = "Fibank", SwiftCode = "FINVBGSF"},
                new BankAccount{BankName = "CCB", SwiftCode = "CECBBGSF"},
                new BankAccount{BankName = "OBB", SwiftCode = "UBBSBGSF"},
                new BankAccount{BankName = "TeximBank", SwiftCode = "TEXIBGSF"},
                new BankAccount{BankName = "DSK Bank", SwiftCode = "STSABGSF"}
            };

            foreach (var account in accounts)
            {
                account.Deposit(7000);

                if (IsValid(account))
                {
                    context.BankAccounts.Add(account);
                }
            }

            context.SaveChanges();
        }

        private static void InsertPaymentMethods(BillsPaymentSystemContext context)
        {
            var methods = new List<PaymentMethod>
            {
                new PaymentMethod{PaymentType = PaymentType.BankAccount, UserId = 1, BankAccountId = 1, CreditCardId = null},
                new PaymentMethod{PaymentType = PaymentType.BankAccount, UserId = 1, BankAccountId = 2, CreditCardId = null},
                new PaymentMethod{PaymentType = PaymentType.BankAccount, UserId = 3, BankAccountId = 3, CreditCardId = null},
                new PaymentMethod{PaymentType = PaymentType.BankAccount, UserId = 3, BankAccountId = 4, CreditCardId = null},
                new PaymentMethod{PaymentType = PaymentType.BankAccount, UserId = 5, BankAccountId = 5, CreditCardId = null},
                new PaymentMethod{PaymentType = PaymentType.BankAccount, UserId = 5, BankAccountId = 6, CreditCardId = null},
                new PaymentMethod{PaymentType = PaymentType.CreditCard, UserId = 2, BankAccountId = null, CreditCardId = 1},
                new PaymentMethod{PaymentType = PaymentType.CreditCard, UserId = 2, BankAccountId = null, CreditCardId = 2},
                new PaymentMethod{PaymentType = PaymentType.CreditCard, UserId = 4, BankAccountId = null, CreditCardId = 3},
                new PaymentMethod{PaymentType = PaymentType.CreditCard, UserId = 4, BankAccountId = null, CreditCardId = 4},
                new PaymentMethod{PaymentType = PaymentType.CreditCard, UserId = 6, BankAccountId = null, CreditCardId = 5},
                new PaymentMethod{PaymentType = PaymentType.CreditCard, UserId = 6, BankAccountId = null, CreditCardId = 6}
            };

            foreach (var method in methods)
            {
                if (IsValid(method))
                {
                    context.PaymentMethods.Add(method);
                }
            }

            context.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            var context = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, context, result, true);
        }
    }
}