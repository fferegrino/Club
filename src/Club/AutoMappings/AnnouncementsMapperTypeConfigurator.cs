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
            Mapper.CreateMap<Club.Models.Entities.Announcement, Club.ViewModels.AnnouncementViewModel>()
                .ForMember(vm => vm.Creator, opt => opt.ResolveUsing(m => m.ClubUserCreator.UserName))
                .ForMember(vm => vm.HumanizedDueDate, opt => opt.ResolveUsing(
                    (r, model) => (model.DueDate - DateTime.Now).Humanize(3, maxUnit: Humanizer.Localisation.TimeUnit.Day)));

            Mapper.CreateMap<Club.Models.Entities.Announcement, Club.ViewModels.AnnouncementCarouselViewModel>()
                .ForMember(vm => vm.ImageUrl, opt => opt.ResolveUsing<AnnouncementImageResolver>())
                .ForMember(vm=> vm.Text , opt => opt.ResolveUsing(m=> m.Text.Truncate(160)))
                //.ForMember(vm => vm.RelatedUrl, opt => opt.ResolveUsing<AnnouncementImageResolver>())
                .ForMember(vm => vm.Creator, opt => opt.ResolveUsing(m => m.ClubUserCreator.UserName))
                .ForMember(vm => vm.HumanizedDueDate, opt => opt.ResolveUsing(
                    (r, model) => (model.DueDate - DateTime.Now).Humanize(3, maxUnit: Humanizer.Localisation.TimeUnit.Day)));

            Mapper.CreateMap<Club.ViewModels.AnnouncementViewModel, Club.Models.Entities.Announcement>()
                .ForMember(m => m.ClubUserCreatorId, opt => opt.Ignore())
                .ForMember(m => m.ClubUserCreator, opt => opt.Ignore())
                ;
        }



        public class AnnouncementImageResolver : ValueResolver<Club.Models.Entities.Announcement, string>
        {
            protected override string ResolveCore(Club.Models.Entities.Announcement source)
            {
                if (String.IsNullOrEmpty(source.ImageUrl))
                    return $"/assets/a/99991231-default.png";
                return $"/assets/a/{source.ImageUrl}.png";
            }
        }
    }
}
