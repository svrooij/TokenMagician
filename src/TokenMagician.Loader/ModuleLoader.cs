/*
* Source: https://github.com/jborean93/PowerShell-ALC/blob/1a5d41dd4012a1553ba8a45aa8c1b9b2ab8a3364/ALCLoader/src/ALCLoader.Shared/LoadContext.cs
* License: unknown (no license)
*/
namespace TokenMagician.Loader;

// AssemblyLoadContext won't work in net472 so we conditionally compile this
// for net5.0 or greater.
#if NET5_0_OR_GREATER
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// A custom AssemblyLoadContext that loads the module assembly in a separate context.
/// </summary>
/// <remarks>Check the <see href="https://github.com/jborean93/PowerShell-ALC/blob/1a5d41dd4012a1553ba8a45aa8c1b9b2ab8a3364/ALCLoader/src/ALCLoader.Shared/LoadContext.cs">source</see> for more information.</remarks>
public class ModuleLoader : AssemblyLoadContext
{
    private static ModuleLoader? _instance;
    private static object _sync = new object();

    private Assembly _thisAssembly;
    private AssemblyName _thisAssemblyName;
    private Assembly _moduleAssembly;
    private string _assemblyDir;

    private ModuleLoader(string mainModulePathAssemblyPath)
        : base (name: "TokenMagician", isCollectible: false)
    {
        _assemblyDir = Path.GetDirectoryName(mainModulePathAssemblyPath) ?? "";
        _thisAssembly = typeof(ModuleLoader).Assembly;
        _thisAssemblyName = _thisAssembly.GetName();
        _moduleAssembly = LoadFromAssemblyPath(mainModulePathAssemblyPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // Checks to see if we are trying to access our current assembly
        // (ALCLoader.Shared). If so return the already loaded assembly object
        // as it provides a common interface between Pwsh and the ALC.
        if (AssemblyName.ReferenceMatchesDefinition(_thisAssemblyName, assemblyName))
        {
            return _thisAssembly;
        }

        // Checks to see if the assembly exists in our path, if so load it in
        // the ALC. Otherwise fallback to the default loading behaviour.
        string asmPath = Path.Join(_assemblyDir, $"{assemblyName.Name}.dll");
        if (File.Exists(asmPath))
        {
            return LoadFromAssemblyPath(asmPath);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Initializes the ModuleLoader and returns the loaded module assembly.
    /// </summary>
    public static Assembly Initialize()
    {
        ModuleLoader? instance = _instance;
        if (instance is not null)
        {
            return instance._moduleAssembly;
        }

        lock (_sync)
        {
            if (_instance is not null)
            {
                return _instance._moduleAssembly;
            }

            string assemblyPath = typeof(ModuleLoader).Assembly.Location;
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);

            // Removes the '.Shared' from the assembly name to refer to our main module.
            string moduleName = assemblyName.Substring(0, assemblyName.Length - 7);
            string modulePath = Path.Combine(
                Path.GetDirectoryName(assemblyPath)!,
                $"{moduleName}.dll"
            );

            // Creates the ALC which loads our module in the ALC and returns
            // the loaded Assembly object for the psm1 to load.
            _instance = new ModuleLoader(modulePath);
            return _instance._moduleAssembly;
        }
    }
}
#endif
