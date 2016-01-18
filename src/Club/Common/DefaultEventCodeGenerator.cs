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
            return GetFixedLengthEventCode(7);
        }

        private static readonly Random Random = new Random();

        public string GetFixedLengthEventCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[Random.Next(s.Length)])
                          .ToArray());

            return result;
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
