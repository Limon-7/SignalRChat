using AutoMapper;
using ChatAPI.Models;
using ChatAPI.Resourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationDto, User>();
            CreateMap<User, UserForReturnDto>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageForReturnDto>()
                .ForMember(s => s.SenderLastName, opt => opt.MapFrom(s => s.Sender.FirstName))
                .ForMember(r => r.RecipientLastName, opt => opt.MapFrom(r => r.Recipient.LastName));
        }
    }
}
