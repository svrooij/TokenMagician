namespace TokenMagician;
using System.Reflection;
using System.Runtime.Loader;

internal class ModuleAssemblyLoadContext : AssemblyLoadContext
{
    private readonly string _dependencyDirPath;

    public ModuleAssemblyLoadContext(string dependencyDirPath): base("TokenMagician")
    {
        _dependencyDirPath = dependencyDirPath;
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        // We do the simple logic here of looking for an assembly of the given name
        // in the configured dependency directory.
        string assemblyPath = Path.Combine(
            _dependencyDirPath,
            $"{assemblyName.Name}.dll");

        if (File.Exists(assemblyPath))
        {
            Console.WriteLine($"Loading assembly {assemblyName.Name} from {assemblyPath}");
            // The ALC must use inherited methods to load assemblies.
            // Assembly.Load*() won't work here.
            return LoadFromAssemblyPath(assemblyPath);
        }

        // For other assemblies, return null to allow other resolutions to continue.
#pragma warning disable CS8603 // Possible null reference return.
        return null;
#pragma warning restore CS8603 // Possible null reference return.
    }
}