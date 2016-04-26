using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Humanizer;

namespace Club.AutoMappings
{
    public class ProblemsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.Problem, Club.ViewModels.ProblemViewModel>()
                .ForMember(vm=> vm.Topic , opt => opt.ResolveUsing(m=>m.Topic.Name))
                ;
        }
    }
}
