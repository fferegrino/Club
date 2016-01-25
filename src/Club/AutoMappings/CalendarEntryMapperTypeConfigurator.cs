using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Humanizer;

namespace Club.AutoMappings
{
    public class CalendarEntryMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Entities.Event, Club.ApiModels.CalendarEntryApiModel>()
                .ForMember(vm => vm.Title, options => options.MapFrom(m => m.Name))
                .ForMember(vm => vm.Start, options => options.MapFrom(m => m.Start.ToString("o")))
                .ForMember(vm => vm.End, options => options.MapFrom(m => m.End.ToString("o")))
                .ForMember(vm => vm.Type, opt => opt.UseValue("event"))
                .ForMember(vm => vm.ClassName, opt => opt.MapFrom(m => "event " + m.Type.ToString().ToLower()))
                .ForMember(vm => vm.Url, options => options.Ignore())
                .ForMember(vm => vm.Color, options => options.Ignore())
                ;

            Mapper.CreateMap<Club.Models.Entities.Announcement, Club.ApiModels.CalendarEntryApiModel>()
                .ForMember(vm => vm.Title, options => options.MapFrom(m => m.Name))
                .ForMember(vm => vm.Start, options => options.MapFrom(m => m.CreatedOn.ToString("o")))
                .ForMember(vm => vm.End, options => options.MapFrom(m => m.DueDate.ToString("o")))
                .ForMember(vm => vm.Description, options => options.MapFrom(m => m.Text))
                .ForMember(vm => vm.Type, opt => opt.UseValue("announcement"))
                .ForMember(vm => vm.ClassName, opt => opt.MapFrom(m => "announcement " + m.Type.ToString().ToLower()))
                .ForMember(vm => vm.Url, options => options.Ignore())
                .ForMember(vm => vm.Color, options => options.Ignore())
                ;
        }
    }

}
