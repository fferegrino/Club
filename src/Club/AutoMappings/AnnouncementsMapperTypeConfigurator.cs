using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.Common.TypeMapping;

namespace Club.AutoMappings
{
    public class AnnouncementsMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap<Club.Models.Announcement, Club.ViewModels.AnnouncementViewModel>();
            Mapper.CreateMap<Club.ViewModels.AnnouncementViewModel, Club.Models.Announcement>();
        }
    }
}
