using System.Reflection;

namespace Application.Test.Helpers;

public static class LogFile
{
    public static IEnumerable<string> LoadEmbeddedLogLines(string resourceName)
    {
        return GetEmbeddedData(resourceName)
            .Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.TrimEnd('\r'));
    }

    private static string GetEmbeddedData(string embeddedResourceEndingIncludingExtension)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(str => str.EndsWith(embeddedResourceEndingIncludingExtension)) ?? string.Empty;
        var resourceStream = assembly
            .GetManifestResourceStream(resourceName);
        if (resourceStream is null)
            return string.Empty;

        using Stream stream = resourceStream;
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}