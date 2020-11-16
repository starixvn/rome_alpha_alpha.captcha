namespace Alpha.Captcha.Helpers
{
    using System.IO;

    internal static class ResourceHelpers
    {
        public static string ReadString(string resourcePath)
        {
            using (var stream = ReadStream(resourcePath))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream ReadStream(string resourcePath)
        {
            return AssemblyHelpers.Current.GetManifestResourceStream(resourcePath);
        }
    }
}