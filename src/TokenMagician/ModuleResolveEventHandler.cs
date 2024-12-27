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
        Console.WriteLine("Module imported");
        AssemblyLoadContext.Default.Resolving += ResolveAssembly;
    }

    /// <summary>
    /// Called when the module is removed from the session, used to cleanup the module.
    /// </summary>
    public void OnRemove(PSModuleInfo module)
    {
        // This is called when the module is removed from the session
        // and can be used to cleanup the module.
        Console.WriteLine("Module removed");
        AssemblyLoadContext.Default.Resolving -= ResolveAssembly;
    }

    private static Assembly? ResolveAssembly(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
    {
        // We only want to resolve the Alc.Engine.dll assembly here.
        // Because this will be loaded into the custom ALC,
        // all of *its* dependencies will be resolved
        // by the logic we defined for that ALC's implementation.
        //
        // Note that we are safe in our assumption that the name is enough
        // to distinguish our assembly here,
        // since it's unique to our module.
        // There should be no other AlcModule.Engine.dll on the system.
        if (!File.Exists(Path.Combine(s_dependencyDirPath, $"{assemblyToResolve.Name}.dll")))
        {
            Console.WriteLine($"Assembly {assemblyToResolve.Name} not found in {s_dependencyDirPath}");
            return null;
        }

        // Allow our ALC to handle the directory discovery concept
        //
        // This is where Alc.Engine.dll is loaded into our custom ALC
        // and then passed through into PowerShell's ALC,
        // becoming the bridge between both
        return s_dependencyAlc.LoadFromAssemblyName(assemblyToResolve);
    }
}