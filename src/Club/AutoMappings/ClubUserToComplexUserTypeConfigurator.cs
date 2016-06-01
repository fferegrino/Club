using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;

namespace Club.AutoMappings
{
    public class ClubUserToComplexUserTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.ClubUser,Club.ApiModels.ComplexUserApiModel>()
                .ForMember(model => model.Username, options => options.MapFrom(viewModel => viewModel.UserName))
                ;
            Mapper.CreateMap<Club.Models.Entities.ClubUser, Club.ViewModels.ComplexUserViewModel>()
                .ForMember(model => model.Username, options => options.MapFrom(viewModel => viewModel.UserName))
                .ForMember(model => model.Level, options => options.MapFrom(viewModel => viewModel.UserLevel.Level))
                ;
        }
    }

}
