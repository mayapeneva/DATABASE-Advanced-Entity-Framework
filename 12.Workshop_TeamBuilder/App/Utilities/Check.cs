namespace App.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public static class Check
    {
        public static void CheckLenght(int length, string[] args)
        {
            if (length != args.Length)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }
        }

        public static bool IsValid(object obj)
        {
            var vContext = new ValidationContext(obj);
            var vResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, vContext, vResults, true);
        }
    }
}