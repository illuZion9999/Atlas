namespace Atlas.Renamer.NameProviders
{
    public interface INameProvider
    {
        string GenerateName(dynamic def);
    }
}