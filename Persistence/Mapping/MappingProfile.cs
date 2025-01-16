using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.AuthDTOs;
using Application.DTOs.CustomerDTOs;
using Application.VMs;
using AutoMapper;
using Domain.Entities;
using Domain.HelperClass;

namespace Persistence.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerDTO, Customer>();
            CreateMap<Customer, CustomerVM>().ReverseMap();
            CreateMap<AddAccounToCustomerDTO, Account>().ReverseMap();
            CreateMap<Account, AccountVM>().ReverseMap();
            CreateMap<Code, CodeVM>().ReverseMap();
            CreateMap<TrackerLog, LogVM>().ReverseMap();
        }
    }
}
