using dnlib.DotNet;

namespace Atlas.Renamer.NameProviders
{
    //TODO: Finish this
    class FileSizeSaving : INameProvider
    {
        public string GenerateName(dynamic def)
        {
            if (def is IMethod met) return GenerateForMethod(met);

            return "";
        }

        static string GenerateForMethod(IMethod method)
        {
            return "";
        }
    }
}