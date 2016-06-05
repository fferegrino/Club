using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;

namespace Club.AutoMappings
{
    public class SignUpToClubUserTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.ViewModels.SignUpViewModel, Club.Models.Entities.ClubUser>()
                .IgnoreAllUnmapped()
                .ForMember(model => model.UserName, options => options.MapFrom(viewModel => viewModel.Username))
                .ForMember(model => model.LastName, options => options.MapFrom(viewModel => viewModel.LastName))
                .ForMember(model => model.FirstName, options => options.MapFrom(viewModel => viewModel.FirstName))
                .ForMember(model => model.Email, options => options.MapFrom(viewModel => viewModel.Email))
                ;
        }
    }

}
