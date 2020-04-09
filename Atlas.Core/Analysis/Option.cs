using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Core.Analysis
{
    public class Option : ICloneable
    {
        public bool Apply { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public object Clone() => MemberwiseClone();

        public override string ToString()
        {
            return $"{(Apply ? "+" : "-")}{Name}({string.Join(", ", Parameters.Select(p => p.Key + ": " + p.Value))})";
        }
    }
}