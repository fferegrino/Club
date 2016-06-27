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
                .ForMember(vm => vm.TermId, opt => opt.ResolveUsing(r => r.Term.Id))
                .ForMember(vm => vm.TermName, opt => opt.ResolveUsing(r => r.Term.Name))
                ;


            Mapper.CreateMap<Club.Models.Entities.EventAttendance, Club.ApiModels.EventAttendanceApiModel>()
                .ForMember(vm => vm.Host, opt => opt.ResolveUsing(m => m.Event.ClubUserHost.UserName))
                .ForMember(vm => vm.Duration, opt => opt.ResolveUsing(
                    (r, model) => (model.Event.End - model.Event.Start).TotalHours))
                .ForMember(vm => vm.TermId, opt => opt.ResolveUsing(r => r.Event.Term.Id))
                .ForMember(vm => vm.TermName, opt => opt.ResolveUsing(r => r.Event.Term.Name))
                .ForMember(vm => vm.User, opt => opt.ResolveUsing(r => r.ClubUser.UserName))
                .ForMember(vm => vm.UserId, opt => opt.ResolveUsing(r => r.ClubUserId))
                .ForMember(vm => vm.Id, opt => opt.ResolveUsing(r => r.Event.Id))
                .ForMember(vm => vm.Description, opt => opt.ResolveUsing(r => r.Event.Description))
                .ForMember(vm => vm.Location, opt => opt.ResolveUsing(r => r.Event.Location))
                .ForMember(vm => vm.Date, opt => opt.ResolveUsing(r => r.Event.Start))
                .ForMember(vm => vm.Type, opt => opt.ResolveUsing(r => r.Event.Type))
                ;



            Mapper.CreateMap<Club.ViewModels.EventViewModel, Club.Models.Entities.Event>()
                .ForMember(m => m.UsersAttending, opt => opt.Ignore())
                .ForMember(m => m.ClubUserHost, opt => opt.Ignore())
                .ForMember(m => m.Term, opt => opt.Ignore())
                .ForMember(m => m.ClubUserHostId, opt => opt.Ignore());


            Mapper.CreateMap<Club.Models.Entities.Event, Club.ApiModels.EventApiModel>()
                .ForMember(vm => vm.Duration, opt => opt.ResolveUsing(
                    (r, model) => (model.End - model.Start).Humanize(3, maxUnit: Humanizer.Localisation.TimeUnit.Day)))
                ;
        }
    }
}
