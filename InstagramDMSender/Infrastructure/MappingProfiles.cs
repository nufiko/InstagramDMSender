using AutoMapper;
using InstagramApiSharp.Classes.Models;
using InstagramDMSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramDMSender.Infrastructure
{
    class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<InstaUserShort, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Pk))
                .ForMember(dest => dest.Private, opt => opt.MapFrom(src => src.IsPrivate));
            CreateMap<InstaUser, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Pk))
                .ForMember(dest => dest.Private, opt => opt.MapFrom(src => src.IsPrivate));
        }
    }
}
