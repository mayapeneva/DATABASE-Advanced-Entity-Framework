using System;
using FastFood.Data;

namespace FastFood.DataProcessor
{
    using System.Linq;

    public static class Bonus
    {
        public const string FailureMessage = "Item {0} not found!";
        private const string SuccessMessage = "{0} Price updated from ${1:F2} to ${2:F2}";

        public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
        {
            var item = context.Items.FirstOrDefault(i => i.Name == itemName);
            if (item == null)
            {
                return string.Format(FailureMessage, itemName);
            }

            var oldPrice = item.Price;
            item.Price = newPrice;
            context.SaveChanges();

            return string.Format(SuccessMessage, itemName, oldPrice, newPrice);
        }
    }
}