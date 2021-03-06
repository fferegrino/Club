﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Club.AutoMappings;
using Club.Common.TypeMapping;
using Microsoft.Extensions.DependencyInjection;

namespace Club.AppConfig
{
    public class MappingConfig
    {


        private static void ConfigureMapping(IEnumerable<IAutoMapperTypeConfigurator> autoMapperTypeConfigurations)
        {
            autoMapperTypeConfigurations.ToList()
                .ForEach(x => x.Config());

            Mapper.AssertConfigurationIsValid();
        }

        internal static void Configure(IServiceCollection services)
        {

            services.AddSingleton<IAutoMapper,AutoMapperAdapter>();

            services.AddSingleton<IAutoMapperTypeConfigurator, AnnouncementsMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, EventsMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, SignUpToClubUserTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, ClubUserToSimpleUserTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, CalendarEntryMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, PagedResponseMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, ClubUserToComplexUserTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, ProblemsMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, TopicsMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, SubmissionsMapperTypeConfigurator>();
            services.AddSingleton<IAutoMapperTypeConfigurator, TermsMapperTypeConfigurator>();
            
            var serviceProvider = services.BuildServiceProvider();
            var configurators = serviceProvider.GetServices<IAutoMapperTypeConfigurator>();
            ConfigureMapping(configurators);
        }
    }
}
