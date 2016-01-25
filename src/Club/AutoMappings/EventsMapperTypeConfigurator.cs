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
            Mapper.CreateMap<Club.Models.Entities.Event, Club.ViewModels.EventViewModel>()
                .ForMember(vm=> vm.Host, opt => opt.ResolveUsing(m=> m.ClubUserHost.UserName))
                .ForMember(vm => vm.EventCodeUrl, opt => opt.Ignore())
                .ForMember(vm => vm.Repeat, opt => opt.Ignore())
                .ForMember(vm => vm.RepeatUntil, opt => opt.Ignore())
                .ForMember(vm => vm.Duration, opt => opt.ResolveUsing(
                    (r,model)=> (model.End - model.Start).Humanize(3,maxUnit: Humanizer.Localisation.TimeUnit.Day)))
                ;


            Mapper.CreateMap<Club.ViewModels.EventViewModel, Club.Models.Entities.Event>()
                .ForMember(m => m.UsersAttending, opt => opt.Ignore())
                .ForMember(m => m.ClubUserHost, opt => opt.Ignore())
                .ForMember(m => m.ClubUserHostId, opt => opt.Ignore());


            Mapper.CreateMap<Club.Models.Entities.Event, Club.ApiModels.EventApiModel>()
                .ForMember(vm => vm.Duration, opt => opt.ResolveUsing(
                    (r, model) => (model.End - model.Start).Humanize(3, maxUnit: Humanizer.Localisation.TimeUnit.Day)))
                ;
        }
    }
}
