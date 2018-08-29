namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using DTOs.Import;
    using Models;
    using Newtonsoft.Json;
    using Data;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        public const string ErrorMsg = "Error: Invalid data.";
        public const string SuccessMsg = "Record {0} successfully imported.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAids = new List<AnimalAid>();
            var result = new StringBuilder();

            var objAnimalAids = JsonConvert.DeserializeObject<AnimalAid[]>(jsonString);
            foreach (var objAnimalAid in objAnimalAids)
            {
                var ifAaExists = animalAids.Any(aa => aa.Name == objAnimalAid.Name);
                if (!IsValid(objAnimalAid) || ifAaExists)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                animalAids.Add(objAnimalAid);
                result.AppendLine(string.Format(SuccessMsg, objAnimalAid.Name));
            }

            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var animals = new List<Animal>();
            var passports = new List<Passport>();
            var result = new StringBuilder();

            var objAnimals = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);
            foreach (var objAnimal in objAnimals)
            {
                var ifPassportExists = animals.Any(a => a.PassportSerialNumber == objAnimal.Passport.SerialNumber);
                if (!IsValid(objAnimal) || !IsValid(objAnimal.Passport)
                    || ifPassportExists || objAnimal.Name.Length < 3
                    || objAnimal.Type.Length < 3)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                animals.Add(Mapper.Map<Animal>(objAnimal));
                passports.Add(Mapper.Map<Passport>(objAnimal.Passport));

                var msg = $"{objAnimal.Name} Passport №: {objAnimal.Passport.SerialNumber}";
                result.AppendLine(string.Format(SuccessMsg, msg));
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var vets = new List<Vet>();
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));
            var objVets = (VetDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            foreach (var objVet in objVets)
            {
                var ifVetExists = vets.Any(v => v.PhoneNumber == objVet.PhoneNumber);
                if (!IsValid(objVet) || ifVetExists)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                vets.Add(Mapper.Map<Vet>(objVet));
                result.AppendLine(string.Format(SuccessMsg, objVet.Name));
            }

            context.Vets.AddRange(vets);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var procedures = new List<Procedure>();
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            var objProcedures = (ProcedureDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            foreach (var objProcedure in objProcedures)
            {
                var ifAnimalAidsValid = true;

                var animal = context.Animals.FirstOrDefault(a => a.PassportSerialNumber == objProcedure.Animal);
                var vet = context.Vets.FirstOrDefault(v => v.Name == objProcedure.Vet);
                var ifParsed = DateTime.TryParseExact(objProcedure.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date);
                if (!IsValid(objProcedure) || !ifParsed
                    || animal == null || vet == null)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                var procedure = new Procedure
                {
                    Animal = animal,
                    Vet = vet,
                    DateTime = date
                };

                var procedureAnimalAids = new List<ProcedureAnimalAid>();
                foreach (var objAnimalAid in objProcedure.AnimalAids)
                {
                    var ifAnimalAidExistsInThisProcedure =
                        procedureAnimalAids.Any(paa => paa.AnimalAid.Name == objAnimalAid.Name);
                    var animalAid = context.AnimalAids.FirstOrDefault(aa => aa.Name == objAnimalAid.Name);
                    if (animalAid == null || ifAnimalAidExistsInThisProcedure)
                    {
                        ifAnimalAidsValid = false;
                        break;
                    }

                    var procedureAnimalAid = new ProcedureAnimalAid
                    {
                        Procedure = procedure,
                        AnimalAid = animalAid
                    };

                    procedureAnimalAids.Add(procedureAnimalAid);
                    procedure.ProcedureAnimalAids.Add(procedureAnimalAid);
                }

                if (!ifAnimalAidsValid)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                procedures.Add(procedure);
                result.AppendLine("Record successfully imported.");
            }

            context.Procedures.AddRange(procedures);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            var vContext = new ValidationContext(obj);
            var vResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, vContext, vResults, true);
        }
    }
}