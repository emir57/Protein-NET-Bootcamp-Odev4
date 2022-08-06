using AutoMapper;
using Pagination.Dto.Concrete;
using Pagination.Entity.Concrete;

namespace Pagination.Business.Helpers
{
    public class BusinessAutoMapperHelper : Profile
    {
        public BusinessAutoMapperHelper()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
        }
    }
}
