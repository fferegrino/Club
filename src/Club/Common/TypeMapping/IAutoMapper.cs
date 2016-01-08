using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Club.Common.TypeMapping
{
    public interface IAutoMapper
    {
        T Map<T>(object toMap);
    }

    public class AutoMapperAdapter : IAutoMapper
    {

        public T Map<T>(object toMap)
        {
            return Mapper.Map<T>(toMap);
        }
    }

}
