using AutoMapper;
using Cafe.Business.Commands.Cafe;
using Cafe.Business.DTOs;
using Cafe.Data.Entities;

namespace Cafe.Business.MappingProfiles
{
    public class CafeProfile : Profile
    {
        public CafeProfile()
        {
            CreateMap<CafeEntity, CafeDto>();
            CreateMap<CreateCafeCommand, CafeEntity>();
            CreateMap<UpdateCafeCommand, CafeEntity>();
        }
    }
}
