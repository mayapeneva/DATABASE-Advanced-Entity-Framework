namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Globalization;
    using Utilities;

    public class CreateEventCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(6, args);

            AuthenticationManager.Authorize();

            var name = args[0];
            var description = args[1];
            var ifStartParsed = DateTime.TryParseExact($"{args[2]} {args[3]}", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime startDate);
            var ifEndParse = DateTime.TryParseExact($"{args[4]} {args[5]}", Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime endDate);
            if (!ifStartParsed || !ifEndParse)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            if (startDate > endDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.StartDateAfterEndDate);
            }

            using (var context = new TeamBuilderContext())
            {
                var currentUser = AuthenticationManager.GetCurrentUser();
                var newEvent = new Event
                {
                    Name = name,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatorId = currentUser.Id
                };

                if (!Check.IsValid(newEvent))
                {
                    throw new ArgumentException(Constants.ErrorMessages.EventNotValid);
                }

                context.Events.Add(newEvent);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Event, name);
        }
    }
}