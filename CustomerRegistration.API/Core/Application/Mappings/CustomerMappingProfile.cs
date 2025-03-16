using AutoMapper;
using CustomerRegistration.API.Core.Application.DTOs;
using CustomerRegistration.API.Core.Domain.Entities;

namespace CustomerRegistration.API.Core.Application.Mappings;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
} 