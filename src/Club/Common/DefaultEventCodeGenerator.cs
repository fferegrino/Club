using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public class DefaultEventCodeGenerator : IEventCodeGenerator
    {
        public string GetCode()
        {
            return Guid.NewGuid().ToString("N");
        }
    }

    public class DummyCodeGenerator : IEventCodeGenerator
    {
        public string GetCode()
        {
            return "1";
        }
    }

}
