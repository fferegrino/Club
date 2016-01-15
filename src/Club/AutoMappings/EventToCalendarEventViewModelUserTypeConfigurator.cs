using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;

namespace Club.AutoMappings
{
    public class EventToCalendarEventViewModelUserTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.Event,Club.ViewModels.CalendarEventViewModel>()
                .ForMember(vm => vm.Title, options => options.MapFrom(m => m.Name))
                .ForMember(vm => vm.Start, options => options.MapFrom(m => m.Start.ToString("o")))
                .ForMember(vm => vm.End, options => options.MapFrom(m => m.End.ToString("o")))
                .ForMember(vm => vm.ClassName, options => options.Ignore())
                .ForMember(vm => vm.Color, options => options.Ignore())
                ;
        }
    }

}
