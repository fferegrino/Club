using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.ApiModels;
using Club.Common.TypeMapping;
using Club.Models;

namespace Club.AutoMappings
{
    public class PagedResponseMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Config()
        {
            Mapper.CreateMap(typeof(QueryResult<>), typeof(PagedDataResponse<>));
        }
    }

}
