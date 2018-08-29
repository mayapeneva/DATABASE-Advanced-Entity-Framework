using System;
using Stations.Data;

namespace Stations.DataProcessor
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using DTOs.Import;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            var stations = new List<Station>();
            var result = new StringBuilder();

            var objStations = JsonConvert.DeserializeObject<Station[]>(jsonString);
            foreach (var objStation in objStations)
            {
                var ifStationExists = stations.Any(s => s.Name == objStation.Name);
                if (!IsValid(objStation) || ifStationExists)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                if (objStation.Town == null)
                {
                    objStation.Town = objStation.Name;
                }

                stations.Add(objStation);
                result.AppendLine(string.Format(SuccessMessage, objStation.Name));
            }

            context.AddRange(stations);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            var classes = new List<SeatingClass>();
            var result = new StringBuilder();

            var objClasses = JsonConvert.DeserializeObject<SeatingClass[]>(jsonString);
            foreach (var objClass in objClasses)
            {
                var ifClassExists = classes.Any(c => c.Name == objClass.Name || c.Abbreviation == objClass.Abbreviation);
                if (!IsValid(objClass) || ifClassExists)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                classes.Add(objClass);
                result.AppendLine(string.Format(SuccessMessage, objClass.Name));
            }

            context.SeatingClasses.AddRange(classes);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            var trains = new List<Train>();
            var result = new StringBuilder();

            var objTrains = JsonConvert.DeserializeObject<TrainDto[]>(jsonString, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            foreach (var objTrain in objTrains)
            {
                var ifTrainExists = trains.Any(t => t.TrainNumber == objTrain.TrainNumber);
                if (!IsValid(objTrain) || ifTrainExists)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var ifAllSeatsValid = true;
                var seats = new List<TrainSeat>();
                foreach (var objSeat in objTrain.Seats)
                {
                    var ifSeatClassExists = context.SeatingClasses.Any(sc =>
                        sc.Name == objSeat.Name && sc.Abbreviation == objSeat.Abbreviation);
                    //var ifSeatClassWithSameNameDiffAbbr = context.SeatingClasses.Any(sc => (sc.Name == objSeat.Name && sc.Abbreviation != objSeat.Abbreviation) || (sc.Name != objSeat.Name && sc.Abbreviation == objSeat.Abbreviation));
                    if (!IsValid(objSeat) || !ifSeatClassExists)
                    {
                        ifAllSeatsValid = false;
                        break;
                    }

                    var seat = new TrainSeat
                    {
                        SeatingClass = context.SeatingClasses.SingleOrDefault(sc =>
                            sc.Name == objSeat.Name && sc.Abbreviation == objSeat.Abbreviation),
                        Quantity = objSeat.Quantity
                    };

                    seats.Add(seat);
                }

                if (!ifAllSeatsValid)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var train = new Train
                {
                    TrainNumber = objTrain.TrainNumber,
                    Type = Enum.Parse<TrainType>(objTrain.Type),
                    TrainSeats = seats
                };

                trains.Add(train);
                result.AppendLine(string.Format(SuccessMessage, train.TrainNumber));
            }

            context.Trains.AddRange(trains);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            var trips = new List<Trip>();
            var result = new StringBuilder();

            var objTrips = JsonConvert.DeserializeObject<TripDto[]>(jsonString, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            foreach (var objTrip in objTrips)
            {
                if (!IsValid(objTrip))
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var ifDeprTimeParsed = DateTime.TryParseExact(objTrip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime departureTime);
                var ifArrTimeParsed = DateTime.TryParseExact(objTrip.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime arrivalTime);
                if (!ifDeprTimeParsed || !ifArrTimeParsed
                    || departureTime > arrivalTime)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var train = context.Trains.SingleOrDefault(t => t.TrainNumber == objTrip.Train);

                var originStation = context.Stations.SingleOrDefault(
                    s => s.Name == objTrip.OriginStation);
                var destinationStation = context.Stations.SingleOrDefault(s => s.Name == objTrip.DestinationStation);
                if (train == null
                    || originStation == null || destinationStation == null)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                TimeSpan timeDifference;
                if (objTrip.TimeDifference != null)
                {
                    timeDifference = TimeSpan.ParseExact(objTrip.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture);
                }

                var trip = new Trip
                {
                    Train = train,
                    OriginStation = originStation,
                    DestinationStation = destinationStation,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    Status = Enum.Parse<TripStatus>(objTrip.Status),
                    TimeDifference = timeDifference
                };

                trips.Add(trip);
                result.AppendLine($"Trip from {trip.OriginStation.Name} to {trip.DestinationStation.Name} imported.");
            }

            context.AddRange(trips);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            var cards = new List<CustomerCard>();
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CustomerCardDto[]), new XmlRootAttribute("Cards"));
            var objCards = (CustomerCardDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            foreach (var objCard in objCards)
            {
                if (!IsValid(objCard))
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var type = Enum.Parse<CardType>(objCard.CardType);
                var card = new CustomerCard
                {
                    Name = objCard.Name,
                    Age = objCard.Age,
                    Type = type
                };

                cards.Add(card);
                result.AppendLine(string.Format(SuccessMessage, card.Name));
            }

            context.Cards.AddRange(cards);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            var tickets = new List<Ticket>();
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            var objTickets = (TicketDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            foreach (var objTicket in objTickets)
            {
                if (!IsValid(objTicket))
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                DateTime.TryParseExact(objTicket.Trip.DepartureTime, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime departureTime);

                var trip = context.Trips
                    .Include(t => t.Train)
                    .ThenInclude(tr => tr.TrainSeats)
                    .SingleOrDefault(t =>
                        t.OriginStation.Name == objTicket.Trip.OriginStation &&
                        t.DestinationStation.Name == objTicket.Trip.DestinationStation &&
                        t.DepartureTime == departureTime);
                if (trip == null)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                var seatAbbr = objTicket.Seat.Substring(0, 2);
                var seatNumber = int.Parse(objTicket.Seat.Substring(2));
                var seatingClass = context.SeatingClasses.SingleOrDefault(sc => sc.Abbreviation == seatAbbr);
                if (seatingClass == null)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }
                if (trip.Train.TrainSeats.Select(ts => ts.SeatingClass).All(sc => sc != seatingClass))
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }
                if (trip.Train.TrainSeats.SingleOrDefault(ts => ts.SeatingClass == seatingClass).Quantity < seatNumber)
                {
                    result.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;
                if (objTicket.CustomerCard != null)
                {
                    var ifCardExists = context.Cards.Any(c => c.Name == objTicket.CustomerCard.Name);

                    if (!ifCardExists)
                    {
                        result.AppendLine(FailureMessage);
                        continue;
                    }
                }

                var ticket = new Ticket
                {
                    Price = objTicket.Price,
                    SeatingPlace = objTicket.Seat,
                    CustomerCard = context.Cards.SingleOrDefault(c => c.Name == objTicket.CustomerCard.Name),
                    Trip = trip
                };

                tickets.Add(ticket);
                result.AppendLine($"Ticket from {objTicket.Trip.OriginStation} to {objTicket.Trip.DestinationStation} departing at {departureTime} imported.");
            }

            context.Tickets.AddRange(tickets);
            context.SaveChanges();

            return result.ToString();
        }

        public static bool IsValid(object obj)
        {
            var vContext = new ValidationContext(obj);
            var vResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, vContext, vResults, true);
        }
    }
}