using AutoMapper;
using Cafe.Data.Entities;
using Cafe.Business.DTOs;

namespace Cafe.Business.MappingProfiles
{
    public class CafeEmployeeProfile : Profile
    {
        public CafeEmployeeProfile()
        {
            CreateMap<CafeEmployeeEntity, CafeEmployeeDto>();

        }
    }
}


