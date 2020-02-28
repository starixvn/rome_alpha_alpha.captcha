namespace Alpha.Captcha.Helpers
{
    using System.IO;

    public static class ResourceHelpers
    {
        public static string LoadEmbeddedResource(string resourcePath)
        {
            using (var stream = AssemblyHelpers.Current.GetManifestResourceStream(resourcePath))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}