using AutoMapper;
using JokesAPI.Models;

namespace JokesAPI.Models.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserInfo, UserInfoDTO>();
            CreateMap<UserInfoDTO, UserInfo>();
        }
    }
}
