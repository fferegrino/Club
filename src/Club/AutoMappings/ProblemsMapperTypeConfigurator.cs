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
    public class ProblemsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.Problem, Club.ViewModels.ProblemViewModel>()
                .ForMember(vm => vm.Topic, opt => opt.ResolveUsing(m => m.Topic.Name))
                .ForMember(vm => vm.Level, opt => opt.ResolveUsing(m => m.Topic.Level.Level))
                .ForMember(vm => vm.LevelId, opt => opt.ResolveUsing(m => m.Topic.Level.Id))
                .ForMember(vm => vm.Site, opt => opt.ResolveUsing<GetHostValueResolver>())
                ;

            Mapper.CreateMap<Club.ViewModels.ProblemViewModel, Club.Models.Entities.Problem>()
                .IgnoreAllUnmapped()
                .ForMember(entity => entity.AddedOn, opt => opt.Ignore())
                .ForMember(entity => entity.ClubUserCreator, opt => opt.Ignore())
                .ForMember(entity => entity.Topic, opt => opt.Ignore())
                .ForMember(entity => entity.ClubUserCreatorId, opt => opt.Ignore());

        }
    }

    public class GetHostValueResolver : ValueResolver<Club.Models.Entities.Problem, string>
    {
        protected override string ResolveCore(Club.Models.Entities.Problem source)
        {
            var uri = new Uri(source.Link);
            return uri.Host;
        }
    }
}
