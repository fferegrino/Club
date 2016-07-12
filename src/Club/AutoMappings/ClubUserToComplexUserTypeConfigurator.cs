using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Club.AutoMappings
{
    public class ClubUserToComplexUserTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.ClubUser, Club.ApiModels.ComplexUserApiModel>()
                .ForMember(model => model.Username, options => options.MapFrom(viewModel => viewModel.UserName))
                ;
            Mapper.CreateMap<Club.Models.Entities.ClubUser, Club.ViewModels.ComplexUserViewModel>()
                .ForMember(model => model.Username, options => options.MapFrom(viewModel => viewModel.UserName))
                .ForMember(model => model.Level, options => options.MapFrom(viewModel => viewModel.UserLevel.Level))
                ;


            Mapper.CreateMap<Club.Models.Entities.ClubUser, Club.ViewModels.EditUserViewModel>()
                .ForMember(model => model.Username, options => options.MapFrom(viewModel => viewModel.UserName))
                .ForMember(model => model.Level, options => options.MapFrom(viewModel => viewModel.UserLevel.Level))
                .ForMember(model => model.LevelId, options => options.MapFrom(viewModel => viewModel.UserLevel.Id))
                .ForMember(model => model.IsAdmin, options => options.Ignore())
                ;
            Mapper.CreateMap<Club.ViewModels.EditUserViewModel, Club.Models.Entities.ClubUser>()
                .IgnoreAllUnmapped()
                .ForMember(model => model.FirstName, options => options.MapFrom(viewModel => viewModel.FirstName))
                .ForMember(model => model.LastName, options => options.MapFrom(viewModel => viewModel.LastName))
                .ForMember(model => model.Email, options => options.MapFrom(viewModel => viewModel.Email))
                .ForMember(model => model.Id, options => options.MapFrom(viewModel => viewModel.Id))
                .ForMember(model => model.IsAdmin, options => options.MapFrom(viewModel => viewModel.IsAdmin))
                .ForMember(model => model.UserName, options => options.MapFrom(viewModel => viewModel.Username))
                .ForMember(model => model.UserLevelId, options => options.MapFrom(viewModel => viewModel.LevelId))
                ;
        }
    }
    
}
