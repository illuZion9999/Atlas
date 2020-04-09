using System;
using System.Collections.Generic;
using Atlas.Core.Analysis;
using dnlib.DotNet;

namespace Atlas.Core.Internal
{
    class ProtectionTargetsFactory : IProtectionTargetsFactory
    {
        public ProtectionTargets Build(string protName, IDictionary<IDnlibDef, OptionList> dict)
        {
            var obj = new ProtectionTargets();

            foreach (var pair in dict)
            {
                foreach (var option in pair.Value)
                {
                    if (!option.Apply || !Compare(protName, option.Name)) continue;

                    obj.Add(pair.Key);
                    obj.Options[pair.Key] = option.Parameters;
                }
            }
            
            return obj;
        }

        static bool Compare(string one, string two) => string.Equals(one, two, StringComparison.OrdinalIgnoreCase);
    }
}