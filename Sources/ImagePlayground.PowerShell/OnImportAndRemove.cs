using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Collections.Generic;
#if NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace ImagePlayground.PowerShell
{

/// <summary>
/// OnModuleImportAndRemove is a class that implements the IModuleAssemblyInitializer and IModuleAssemblyCleanup interfaces.
/// This class is used to handle the assembly resolve event when the module is imported and removed.
/// </summary>
public class OnModuleImportAndRemove : IModuleAssemblyInitializer, IModuleAssemblyCleanup {
    /// <summary>
    /// OnImport is called when the module is imported.
    /// </summary>
    public void OnImport() {
        if (IsNetFramework()) {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }
#if NET5_0_OR_GREATER
        else {
            _ = _alc.LoadFromAssemblyPath(typeof(OnModuleImportAndRemove).Assembly.Location);
            AssemblyLoadContext.Default.Resolving += ResolveAlc;
        }
#endif
    }

    /// <summary>
    /// OnRemove is called when the module is removed.
    /// </summary>
    /// <param name="module"></param>
    public void OnRemove(PSModuleInfo module) {
        if (IsNetFramework()) {
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
        }
#if NET5_0_OR_GREATER
        else {
            AssemblyLoadContext.Default.Resolving -= ResolveAlc;
        }
#endif
    }

    /// <summary>
    /// ResolveAssembly is a method that handles the AssemblyResolve event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    private static Assembly? ResolveAssembly(object? sender, ResolveEventArgs args) {
        var libDirectory = Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location);
        var directoriesToSearch = new List<string>();

        if (!string.IsNullOrEmpty(libDirectory)) {
            directoriesToSearch.Add(libDirectory);
            if (Directory.Exists(libDirectory)) {
                directoriesToSearch.AddRange(Directory.GetDirectories(libDirectory, "*", SearchOption.AllDirectories));
            }
        }

        var requestedAssemblyName = new AssemblyName(args.Name).Name + ".dll";

        foreach (var directory in directoriesToSearch) {
            var assemblyPath = Path.Combine(directory, requestedAssemblyName);

            if (File.Exists(assemblyPath)) {
                try {
                    return Assembly.LoadFrom(assemblyPath);
                } catch (Exception ex) {
                    Console.WriteLine($"Failed to load assembly from {assemblyPath}: {ex.Message}");
                }
            }
        }

        return null;
    }

#if NET5_0_OR_GREATER
    private static readonly string _assemblyDir =
        Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location)!;

    private static readonly LoadContext _alc = new LoadContext(_assemblyDir);

    private static Assembly? ResolveAlc(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve) {
        string asmPath = Path.Combine(_assemblyDir, $"{assemblyToResolve.Name}.dll");
        if (IsSatisfyingAssembly(assemblyToResolve, asmPath)) {
            return _alc.LoadFromAssemblyName(assemblyToResolve);
        } else {
            return null;
        }
    }

    private static bool IsSatisfyingAssembly(AssemblyName requiredAssemblyName, string assemblyPath) {
        if (requiredAssemblyName.Name == typeof(OnModuleImportAndRemove).Assembly.GetName().Name ||
            !File.Exists(assemblyPath)) {
            return false;
        }

        AssemblyName asmToLoadName = AssemblyName.GetAssemblyName(assemblyPath);

        return string.Equals(asmToLoadName.Name, requiredAssemblyName.Name, StringComparison.OrdinalIgnoreCase)
            && asmToLoadName.Version >= requiredAssemblyName.Version;
    }
#endif

    /// <summary>
    /// Determine if the current runtime is .NET Framework
    /// </summary>
    /// <returns></returns>
    private bool IsNetFramework() {
        return System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
    }

}

}

