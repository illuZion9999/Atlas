using dnlib.DotNet;

namespace Atlas.Core.ReferenceResolving.Resolvers
{
    class NormalResolver : IReferenceResolver
    {
        public bool ResolveReference(AssemblyRef reference, ModuleDef module, AssemblyResolver resolver)
        {
            try
            {
                resolver.ResolveThrow(reference, module);
                return true;
            }
            catch (AssemblyResolveException)
            {
                return false;
            }
        }
    }
}