using dnlib.DotNet;

namespace Atlas.Core.ReferenceResolving
{
    interface IReferenceResolver
    {
        bool ResolveReference(AssemblyRef reference, ModuleDef module, AssemblyResolver resolver);
    }
}