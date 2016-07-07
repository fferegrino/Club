using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Club.ViewModels;
using Humanizer;

namespace Club.AutoMappings
{
    public class TermsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.Term, Club.ViewModels.TermViewModel>()
                ;
            Mapper.CreateMap<Club.ViewModels.TermViewModel, Club.Models.Entities.Term>()
                .ForMember(m => m.Events, opt => opt.Ignore())
                ;

            //Mapper.CreateMap<Club.ViewModels.ProblemViewModel, Club.Models.Entities.Problem>()
            //    .IgnoreAllUnmapped()
            //    .ForMember(entity => entity.AddedOn, opt => opt.Ignore())
            //    .ForMember(entity => entity.ClubUserCreator, opt => opt.Ignore())
            //    .ForMember(entity => entity.Topic, opt => opt.Ignore())
            //    .ForMember(entity => entity.ClubUserCreatorId, opt => opt.Ignore());

        }
    }
    
}
