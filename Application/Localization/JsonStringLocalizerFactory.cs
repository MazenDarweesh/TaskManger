using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;

//Key Responsibilities of JsonStringLocalizerFactory
//1.	Initialize with Resources Path
//2.	Create Localizer by Type
//3.	Create Localizer by Base Name and Location

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly string _resourcesPath;

    public JsonStringLocalizerFactory()
    {
        var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
        _resourcesPath = Path.GetFullPath(filePath);
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        // Creates a generic type JsonStringLocalizer<resourceSource>.
        var localizerType = typeof(JsonStringLocalizer<>).MakeGenericType(resourceSource);
        // Creates an instance of the JsonStringLocalizer<T> with the specified resources path.
        return (IStringLocalizer)Activator.CreateInstance(localizerType, _resourcesPath);
    }

    //baseName: The base name of the resource type
    public IStringLocalizer Create(string baseName, string location)
    {
        var resourceSource = Type.GetType(baseName);
        if (resourceSource == null)
        {
            throw new ArgumentException($"Type '{baseName}' not found.");
        }
        return Create(resourceSource);
    }
}
//How It All Fits Together
//1.	Factory Initialization: The JsonStringLocalizerFactory is initialized with the path to the JSON resource files.
//2.	Localizer Creation: When a localizer is needed, the factory creates an instance of JsonStringLocalizer<T> with the appropriate type and resources path.
//3.	Dependency Injection: The factory and localizers are registered with the dependency injection system, allowing them to be injected into services and controllers.