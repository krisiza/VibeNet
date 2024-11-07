using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VibeNet.Core.ViewModels;
using VibeNetInfrastucture.Data.Models;
using VibeNetInfrastucture.Validations;
using static VibeNetInfrastucture.Validations.ValidationConstants.DateTimeFormat;

namespace VibeNet.Core.Mapping
{
    public class VibeNetProfile : Profile
    {
        public VibeNetProfile() 
        {
            //Entity To ViewModel
            CreateMap<VibeNetUser, VibeNetUserRegisterViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.VibeNetUserId)))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday.ToString(Format, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn.ToString(Format, CultureInfo.InvariantCulture)))
                /*.ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => string.Join("", src.ProfilePicture)))*/;

        }
    }
}
