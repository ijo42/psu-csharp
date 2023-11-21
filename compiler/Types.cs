using System.Collections.Generic;

namespace Компилятор
{
    class Types
    {
        public static readonly Dictionary<string, byte> Kw = new()
        {
            ["real"] = 1,
            ["integer"] = 2
        };
        
    }
}