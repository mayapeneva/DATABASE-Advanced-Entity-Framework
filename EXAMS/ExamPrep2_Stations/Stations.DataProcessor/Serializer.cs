using System;
using Stations.Data;

namespace Stations.DataProcessor
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using DTOs.Export;
    using Microsoft.EntityFrameworkCore;
    using Models.Enums;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportDelayedTrains(StationsDbContext context, string dateAsString)
        {
            var date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var trains = context.Trains
                .Where(t =>
                t.Trips.Any(tr => tr.Status == TripStatus.Delayed
                        && tr.DepartureTime <= date))
                .Select(t => new
                {
                    t.TrainNumber,
                    DelayedTrips = t.Trips.Where(tr => tr.Status == TripStatus.Delayed && tr.DepartureTime <= date).ToArray()
                })
                .Select(t => new DelayedTrainDto
                {
                    TrainNumber = t.TrainNumber,
                    DelayedTimes = t.DelayedTrips.Length,
                    MaxDelayedTime = t.DelayedTrips.Max(tr => tr.TimeDifference).Value.ToString()
                })
                .OrderByDescending(t => t.DelayedTimes)
                .ThenByDescending(t => t.MaxDelayedTime)
                .ThenBy(t => t.TrainNumber)
                .ToArray();

            return JsonConvert.SerializeObject(trains, Formatting.Indented);
        }

        public static string ExportCardsTicket(StationsDbContext context, string cardType)
        {
            var cardT = Enum.Parse<CardType>(cardType);
            var cards = context.Cards
                .Where(c => c.Type == cardT && c.BoughtTickets.Any())
                .OrderBy(c => c.Name)
                .Select(c => new CardExportDto
                {
                    Name = c.Name,
                    Type = c.Type.ToString(),
                    Tickets = c.BoughtTickets.Select(t => new TicketExportDto
                    {
                        OriginStation = t.Trip.OriginStation.Name,
                        DestinationStation = t.Trip.DestinationStation.Name,
                        DepartureTime = t.Trip.DepartureTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    }).ToArray()
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(CardExportDto[]), new XmlRootAttribute("Cards"));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, cards, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return writer.ToString();
            }
        }
    }
}