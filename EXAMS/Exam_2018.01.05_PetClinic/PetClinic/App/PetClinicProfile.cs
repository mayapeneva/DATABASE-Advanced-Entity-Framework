namespace PetClinic.App
{
    using System;
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using DataProcessor.DTOs.Export;
    using DataProcessor.DTOs.Import;
    using Models;

    public class PetClinicProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public PetClinicProfile()
        {
            this.CreateMap<AnimalDto, Animal>();
            this.CreateMap<PassportDto, Passport>()
                .ForMember(dto => dto.RegistrationDate, dest => dest.MapFrom(p => DateTime.ParseExact(p.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<VetDto, Vet>();

            this.CreateMap<Animal, AnimalExportDto>()
                .ForMember(e => e.OwnerName, d => d.MapFrom(a => a.Passport.OwnerName))
                .ForMember(e => e.AnimalName, d => d.MapFrom(a => a.Name))
                .ForMember(e => e.SerialNumber, d => d.MapFrom(a => a.PassportSerialNumber))
                .ForMember(e => e.RegisteredOn, d => d.MapFrom(a => a.Passport.RegistrationDate));

            //this.CreateMap<Procedure, ProcedureExportDto>()
            //    .ForMember(e => e.Passport, d => d.MapFrom(p => p.Animal.PassportSerialNumber))
            //    .ForMember(e => e.OwnerNumber, d => d.MapFrom(p => p.Animal.Passport.OwnerName))
            //    .ForMember(e => e.DateTime, d => d.MapFrom(p => p.DateTime.ToString("dd-MM-yyyy")))
            //    .ForMember(e => e.AnimalAids, d => d.MapFrom(p => p.ProcedureAnimalAids.Select(paa => paa.AnimalAid)));
            //this.CreateMap<AnimalAid, AnimalAidExportDto>();
        }
    }
}