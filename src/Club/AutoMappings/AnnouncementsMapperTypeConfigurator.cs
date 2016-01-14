using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;
using Humanizer;

namespace Club.AutoMappings
{
    public class AnnouncementsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Announcement, Club.ViewModels.AnnouncementViewModel>()
                .ForMember(vm => vm.HumanizedDueDate, opt => opt.ResolveUsing(
                    (r, model) => (model.DueDate - DateTime.Now).Humanize(15, maxUnit: Humanizer.Localisation.TimeUnit.Hour)));
            Mapper.CreateMap<Club.ViewModels.AnnouncementViewModel, Club.Models.Announcement>();
        }
    }
}
