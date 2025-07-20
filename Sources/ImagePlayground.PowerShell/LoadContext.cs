#if NET5_0_OR_GREATER
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ImagePlayground.PowerShell;

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
#endif
