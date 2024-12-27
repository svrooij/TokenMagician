using System.Management.Automation;
using System.Reflection;
using System.Runtime.Loader;

namespace TokenMagician;

/// <summary>
/// A class that initializes the module when it is imported into the session.
/// </summary>
public class ModuleResolveEventHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
{
    // Get the path of the dependency directory.
    // In this case we find it relative to the AlcModule.Cmdlets.dll location
    private static readonly string s_dependencyDirPath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

    private static readonly ModuleAssemblyLoadContext s_dependencyAlc =
        new ModuleAssemblyLoadContext(s_dependencyDirPath);

    /// <summary>
    /// Called when the module is imported into the session, used to hook into the AssemblyLoadContext
    /// </summary>
    public void OnImport()
    {
        // This is called when the module is imported into the session
        // and can be used to initialize the module.
        Console.WriteLine("Attaching custom AssemblyLoadContext");
        AssemblyLoadContext.Default.Resolving += ResolveAssembly;
    }

    /// <summary>
    /// Called when the module is removed from the session, used to cleanup the module.
    /// </summary>
    public void OnRemove(PSModuleInfo module)
    {
        // This is called when the module is removed from the session
        // and can be used to cleanup the module.
        Console.WriteLine("Removing custom AssemblyLoadContext");
        AssemblyLoadContext.Default.Resolving -= ResolveAssembly;
    }

    private static Assembly? ResolveAssembly(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
    {
        // Check if the assembly is already loaded in the default ALC
        // And return it no matter of the requested version.
        // This will "redirect" the request assembly to the version already loaded.
        var assembly = defaultAlc.Assemblies.FirstOrDefault(assembly => assembly.GetName().Name == assemblyToResolve.Name);
        if (assembly != null)
        {
            var assemblyName = assembly.GetName();
            if(assemblyName.Version != assemblyToResolve.Version)
            {
                Console.WriteLine($"Assembly {assemblyToResolve.Name} v{assemblyName.Version} already loaded in the default ALC (redirect from v{assemblyToResolve.Version})");
            } else {
                Console.WriteLine($"Assembly {assemblyToResolve.Name} v{assemblyName.Version} already loaded in the default ALC");
            }
            return assembly;
        }

        // Check if we can find the assembly in the dependency directory
        // return null to let the default ALC handle the resolution
        if (!File.Exists(Path.Combine(s_dependencyDirPath, $"{assemblyToResolve.Name}.dll")))
        {
            Console.WriteLine($"Assembly {assemblyToResolve.Name} not found in {s_dependencyDirPath}");
            return null;
        }

        return s_dependencyAlc.LoadFromAssemblyName(assemblyToResolve);
    }
}