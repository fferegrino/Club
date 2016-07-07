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
    public class TopicsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.Topic, Club.ViewModels.TopicViewModel>()
                .ForMember(vm => vm.UserLevel, opt => opt.ResolveUsing(m => m.Level.Level))
                ;
            Mapper.CreateMap<Club.ViewModels.TopicViewModel,Club.Models.Entities.Topic>()
                .ForMember(en => en.Level, opt => opt.Ignore())
                .ForMember(en => en.Problems, opt => opt.Ignore());
        }
    }
}
