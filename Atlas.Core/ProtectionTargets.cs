using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace Atlas.Core
{
    public class ProtectionTargets : List<IDnlibDef>
    {
        internal ProtectionTargets() { }
        internal IDictionary<IMDTokenProvider, IDictionary<string, string>> Options = new Dictionary<IMDTokenProvider, IDictionary<string, string>>();

        public string GetOption(IMDTokenProvider target, string key, string def = default)
        {
            if (!Options.ContainsKey(target)) return def;

            var value = Options[target];
            return !value.ContainsKey(key) ? def : value[key];
        }
        
        public T GetOption<T>(IDnlibDef target, string key, T def = default)
        {
            return (T)Convert.ChangeType((object)GetOption(target, key) ?? def, typeof(T));
        }
    }
}