namespace SoftJail.DataProcessor
{
    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMsg = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();
            var depts = new List<Department>();

            var objDpts = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);
            foreach (var objDpt in objDpts)
            {
                if (!IsValid(objDpt))
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                var ifAllCellsValid = true;
                var cells = new List<Cell>();
                foreach (var objCell in objDpt.Cells)
                {
                    if (!IsValid(objCell))
                    {
                        ifAllCellsValid = false;
                        break;
                    }

                    cells.Add(Mapper.Map<Cell>(objCell));
                }

                if (!ifAllCellsValid)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                var dept = Mapper.Map<Department>(objDpt);
                dept.Cells = cells;
                depts.Add(dept);

                result.AppendLine($"Imported {objDpt.Name} with {objDpt.Cells.Count} cells");
            }

            context.Departments.AddRange(depts);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();
            var prisoners = new List<Prisoner>();

            var objPrisoners = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);
            foreach (var objPrisoner in objPrisoners)
            {
                if (!IsValid(objPrisoner))
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                var incDate = DateTime.ParseExact(objPrisoner.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
                DateTime relDate = default(DateTime);
                if (objPrisoner.ReleaseDate != null)
                {
                    relDate = DateTime.ParseExact(objPrisoner.ReleaseDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                }

                var ifAllMailsValid = true;
                var mails = new List<Mail>();
                foreach (var objMail in objPrisoner.Mails)
                {
                    if (!IsValid(objMail))
                    {
                        ifAllMailsValid = false;
                        break;
                    }

                    mails.Add(Mapper.Map<Mail>(objMail));
                }

                if (!ifAllMailsValid)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                var prisoner = new Prisoner
                {
                    FullName = objPrisoner.FullName,
                    Nickname = objPrisoner.Nickname,
                    Age = objPrisoner.Age,
                    IncarcerationDate = incDate,
                    ReleaseDate = relDate,
                    Bail = objPrisoner.Bail ?? 0,
                    CellId = objPrisoner.CellId,
                    Mails = mails
                };

                prisoners.Add(prisoner);

                result.AppendLine($"Imported {objPrisoner.FullName} {objPrisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return result.ToString();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var result = new StringBuilder();
            var officers = new List<Officer>();

            var serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));
            var objOfficers = (OfficerDto[])serializer.Deserialize(new StringReader(xmlString));
            foreach (var objOfficer in objOfficers)
            {
                var ifPositionParsed = Enum.TryParse<Position>(objOfficer.Position, out Position position);
                var ifWeaponParsed = Enum.TryParse<Weapon>(objOfficer.Weapon, out Weapon weapon);
                if (!IsValid(objOfficer)
                    || !ifPositionParsed || !ifWeaponParsed)
                {
                    result.AppendLine(ErrorMsg);
                    continue;
                }

                var officer = new Officer
                {
                    FullName = objOfficer.FullName,
                    Salary = objOfficer.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = objOfficer.DepartmentId,
                };

                foreach (var objPrisoner in objOfficer.Prisoners)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner { PrisonerId = objPrisoner.Id });
                }

                officers.Add(officer);

                result.AppendLine($"Imported {objOfficer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
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