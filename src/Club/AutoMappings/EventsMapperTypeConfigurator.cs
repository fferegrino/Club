using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;

namespace Club.AutoMappings
{
    public class EventsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Event, Club.ViewModels.EventViewModel>();
            Mapper.CreateMap<Club.ViewModels.EventViewModel, Club.Models.Event>();
        }
    }
}
