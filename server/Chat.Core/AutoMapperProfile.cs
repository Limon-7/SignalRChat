using AutoMapper;
using Chat.Core.Resourses;
using Chat.Data.Entities;
using System;

namespace Chat.Core
{
	public class AutoMapperProfile:Profile
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

    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
    //call anywhere to mapping
    //var destination = Mapping.Mapper.Map<Destination>(yourSourceInstance);
}
