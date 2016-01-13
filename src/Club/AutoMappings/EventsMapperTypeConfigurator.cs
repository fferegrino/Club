using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Humanizer;

namespace Club.AutoMappings
{
    public class EventsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Event, Club.ViewModels.EventViewModel>()
                .ForMember(vm => vm.EventCodeUrl, opt => opt.Ignore())
                .ForMember(vm => vm.Duration, opt => opt.ResolveUsing(
                    (r,model)=>
                    {
                        return (model.End - model.Start).Humanize(15,maxUnit: Humanizer.Localisation.TimeUnit.Hour);
                    }))
                ;


            Mapper.CreateMap<Club.ViewModels.EventViewModel, Club.Models.Event>();
        }
    }
}
