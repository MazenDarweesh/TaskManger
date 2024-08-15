using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;

public class JsonStringLocalizer<T> : IStringLocalizer<T>
{
    private readonly ConcurrentDictionary<string, JObject> _localizationData;
    private readonly string _resourcesPath;

    public JsonStringLocalizer(string resourcesPath)
    {
        _resourcesPath = resourcesPath;
        _localizationData = new ConcurrentDictionary<string, JObject>();
    }

    private JObject GetLocalizationData(string culture)
    {
        // Checks if the data is already cached.
        if (_localizationData.TryGetValue(culture, out var localizationData))
        {
            return localizationData;
        }

        var filePath = Path.Combine(_resourcesPath, $"{typeof(T).Name}.{culture}.json");
        Console.WriteLine($"Looking for resource file at: {filePath}"); // Add this line for logging

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            localizationData = JObject.Parse(json);
            _localizationData[culture] = localizationData;
        }
        else
        {
            localizationData = new JObject();
        }

        return localizationData;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var localizationData = GetLocalizationData(culture);
            var value = localizationData[name]?.ToString() ?? name;
            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = this[name].Value;
            var value = string.Format(format, arguments);
            return new LocalizedString(name, value);
        }
    }
    // returns all localized strings for the current culture.
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentCulture.Name;
        var localizationData = GetLocalizationData(culture);

        foreach (var item in localizationData)
        {
            yield return new LocalizedString(item.Key, item.Value?.ToString() ?? string.Empty);
        }
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
    {
        return new JsonStringLocalizer<T>(_resourcesPath);
    }
}
