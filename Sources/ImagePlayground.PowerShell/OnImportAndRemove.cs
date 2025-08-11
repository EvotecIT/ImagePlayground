using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Collections.Generic;
#if NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace ImagePlayground.PowerShell {

    /// <summary>
    /// OnModuleImportAndRemove is a class that implements the IModuleAssemblyInitializer and IModuleAssemblyCleanup interfaces.
    /// This class is used to handle the assembly resolve event when the module is imported and removed.
    /// </summary>
    public class OnModuleImportAndRemove : IModuleAssemblyInitializer, IModuleAssemblyCleanup {

#if NET5_0_OR_GREATER
        // ==================================================================================
        // PowerShell 7+ (.NET Core/5+) Implementation
        // ==================================================================================

        private static readonly string _assemblyDir =
            Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location)!;

        private static readonly LoadContext _alc = new LoadContext(_assemblyDir);

        /// <summary>
        /// OnImport is called when the module is imported (PS 7+).
        /// </summary>
        public void OnImport() {
            // Use AssemblyLoadContext for .NET Core/5+
            _ = _alc.LoadFromAssemblyPath(typeof(OnModuleImportAndRemove).Assembly.Location);
            AssemblyLoadContext.Default.Resolving += ResolveAlc;
        }

        /// <summary>
        /// OnRemove is called when the module is removed (PS 7+).
        /// </summary>
        /// <param name="module"></param>
        public void OnRemove(PSModuleInfo module) {
            // Unsubscribe from AssemblyLoadContext.Resolving
            AssemblyLoadContext.Default.Resolving -= ResolveAlc;
        }

        /// <summary>
        /// ResolveAlc handles the AssemblyLoadContext.Resolving event for .NET Core/5+ (PS 7+).
        /// </summary>
        /// <param name="defaultAlc">The default AssemblyLoadContext.</param>
        /// <param name="assemblyToResolve">The assembly name to resolve.</param>
        /// <returns>The resolved assembly, or null if it could not be resolved.</returns>
        private static Assembly? ResolveAlc(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve) {
            string asmPath = Path.Combine(_assemblyDir, $"{assemblyToResolve.Name}.dll");
            if (IsSatisfyingAssembly(assemblyToResolve, asmPath)) {
                return _alc.LoadFromAssemblyName(assemblyToResolve);
            } else {
                return null;
            }
        }

        /// <summary>
        /// Checks if the assembly at the specified path satisfies the required assembly name and version.
        /// Used by PS 7+ assembly resolution.
        /// </summary>
        /// <param name="requiredAssemblyName">The required assembly name.</param>
        /// <param name="assemblyPath">The path to the assembly.</param>
        /// <returns>True if the assembly satisfies the requirements; otherwise, false.</returns>
        private static bool IsSatisfyingAssembly(AssemblyName requiredAssemblyName, string assemblyPath) {
            if (requiredAssemblyName.Name == typeof(OnModuleImportAndRemove).Assembly.GetName().Name ||
                !File.Exists(assemblyPath)) {
                return false;
            }

            AssemblyName asmToLoadName = AssemblyName.GetAssemblyName(assemblyPath);

            return string.Equals(asmToLoadName.Name, requiredAssemblyName.Name, StringComparison.OrdinalIgnoreCase)
                && asmToLoadName.Version >= requiredAssemblyName.Version;
        }

        /// <summary>
        /// Internal LoadContext class for Assembly Load Context isolation in PS 7+.
        /// This creates a separate context for loading module dependencies.
        /// </summary>
        internal class LoadContext : AssemblyLoadContext {
            private readonly string _assemblyDir;

            public LoadContext(string assemblyDir)
                : base("ImagePlayground", isCollectible: false) {
                _assemblyDir = assemblyDir;
            }

            protected override Assembly? Load(AssemblyName assemblyName) {
                string asmPath = Path.Combine(_assemblyDir, $"{assemblyName.Name}.dll");
                return File.Exists(asmPath) ? LoadFromAssemblyPath(asmPath) : null;
            }
        }

#else
        // ==================================================================================
        // PowerShell 5.1 (.NET Framework) Implementation
        // ==================================================================================

        /// <summary>
        /// OnImport is called when the module is imported (PS 5.1).
        /// </summary>
        public void OnImport() {
            // Use AppDomain.AssemblyResolve for .NET Framework
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        /// <summary>
        /// OnRemove is called when the module is removed (PS 5.1).
        /// </summary>
        /// <param name="module"></param>
        public void OnRemove(PSModuleInfo module) {
            // Unsubscribe from AppDomain.AssemblyResolve
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
        }

        /// <summary>
        /// ResolveAssembly handles the AssemblyResolve event for .NET Framework (PS 5.1).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly? ResolveAssembly(object? sender, ResolveEventArgs args) {
            var requestedAssemblyName = new AssemblyName(args.Name);

            // First check if the assembly is already loaded
            foreach (Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies()) {
                if (loadedAssembly.GetName().Name == requestedAssemblyName.Name) {
                    // Check if the loaded version satisfies the requirement
                    if (loadedAssembly.GetName().Version >= requestedAssemblyName.Version) {
                        return loadedAssembly;
                    }
                }
            }

            var libDirectory = Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location);
            var directoriesToSearch = new List<string>();

            if (!string.IsNullOrEmpty(libDirectory)) {
                directoriesToSearch.Add(libDirectory);
                if (Directory.Exists(libDirectory)) {
                    directoriesToSearch.AddRange(Directory.GetDirectories(libDirectory, "*", SearchOption.AllDirectories));
                }
            }

            var requestedAssemblyFileName = requestedAssemblyName.Name + ".dll";

            foreach (var directory in directoriesToSearch) {
                var assemblyPath = Path.Combine(directory, requestedAssemblyFileName);

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
#endif

    }

}