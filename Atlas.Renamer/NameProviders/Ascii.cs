using System;
using System.Linq;

namespace Atlas.Renamer.NameProviders
{
    class Ascii : INameProvider
    {
        readonly Random _r = new Random();
        
        public string GenerateName(dynamic def)
        {
            return new string(Enumerable.Repeat(string.Empty, 16).Select(s => (char)_r.Next(65, 80)).ToArray());
        }
    }
}