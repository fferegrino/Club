using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public class PrimitiveTypeParser
    {
        public static T Parse<T>(string val)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var result = converter.ConvertFromString(val);
            return (T)result;
        }
    }
}
