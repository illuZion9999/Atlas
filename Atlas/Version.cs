namespace Atlas
{
    public static class Version
    {
        const string Major = "0";
        const string Minor = "0";
        const string Patch = "7";
        const string Postfix = "-alpha";

        public static string Get() => $"{Major}.{Minor}.{Patch}{Postfix}";
    }
}