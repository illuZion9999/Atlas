using Atlas.Core.Analysis.Parser;
using dnlib.DotNet;

namespace Atlas.Core.Analysis
{
    static class HelperExtensions
    {
        static void IsObfuscationAttributeThrow(this CustomAttribute attribute)
        {
            if (attribute.TypeFullName != "System.Reflection.ObfuscationAttribute")
                throw new ObfuscationAttributeParserException($"Attribute wasn't ObfuscationAttribute, instead {attribute.TypeFullName}");
        }
        
        internal static bool Exclude(this CustomAttribute attribute)
        {
            attribute.IsObfuscationAttributeThrow();
            foreach (var arg in attribute.NamedArguments)
            {
                if (arg.Name != "Exclude")
                    continue;

                return (bool)arg.Argument.Value;
            }

            return true;
        }

        internal static string Feature(this CustomAttribute attribute)
        {
            attribute.IsObfuscationAttributeThrow();
            foreach (var arg in attribute.NamedArguments)
            {
                if (arg.Name != "Feature")
                    continue;

                return (UTF8String)arg.Argument.Value;
            }
                
            return string.Empty;
        }

        internal static bool ApplyToMembers(this CustomAttribute attribute)
        {
            attribute.IsObfuscationAttributeThrow();
            foreach (var arg in attribute.NamedArguments)
            {
                if (arg.Name != "ApplyToMembers")
                    continue;

                return (bool)arg.Argument.Value;
            }

            return true;
        }
        
        internal static bool StripAfterObfuscation(this CustomAttribute attribute)
        {
            attribute.IsObfuscationAttributeThrow();
            foreach (var arg in attribute.NamedArguments)
            {
                if (arg.Name != "StripAfterObfuscation")
                    continue;

                return (bool)arg.Argument.Value;
            }

            return true;
        }
    }
}