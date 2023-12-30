using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Reflection;

public class OnModuleImportAndRemove : IModuleAssemblyInitializer, IModuleAssemblyCleanup {
    public void OnImport() {
#if FRAMEWORK
        AppDomain.CurrentDomain.AssemblyResolve += MyResolveEventHandler;
#endif
    }

    public void OnRemove(PSModuleInfo module) {
#if FRAMEWORK
        AppDomain.CurrentDomain.AssemblyResolve -= MyResolveEventHandler;
#endif
    }

    private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args) {
        // These are known to be problematic in .NET Framework, force it to use our packaged dlls.
        //if (args.Name.StartsWith("System.Memory,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.Memory.dll");
        //    return Assembly.LoadFile(binPath);
        //} else if (args.Name.StartsWith("System.Runtime.CompilerServices.Unsafe,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.Runtime.CompilerServices.Unsafe.dll");
        //    return Assembly.LoadFile(binPath);
        //} else if (args.Name.StartsWith("System.Numerics.Vectors,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.Numerics.Vectors.dll");
        //    return Assembly.LoadFile(binPath);
        //} else if (args.Name.StartsWith("System.Drawing.Common,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.Drawing.Common.dll");
        //    return Assembly.LoadFile(binPath);
        //} else if (args.Name.StartsWith("System.Buffers,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.Buffers.dll");
        //    return Assembly.LoadFile(binPath);
        //} else if (args.Name.StartsWith("System.ValueTuple,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.ValueTuple.dll");
        //    return Assembly.LoadFile(binPath);
        //} else if (args.Name.StartsWith("System.Text.Encoding.CodePages,")) {
        //    string binPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), "System.Text.Encoding.CodePagesdll");
        //    return Assembly.LoadFile(binPath);
        //}

        var assemblyList = new List<(string, string)>
        {
            ("System.Memory,", "System.Memory.dll"),
            ("System.Runtime.CompilerServices.Unsafe,", "System.Runtime.CompilerServices.Unsafe.dll"),
            ("System.Text.Encoding.CodePages,", "System.Text.Encoding.CodePages.dll"),
            ("System.Buffers,", "System.Buffers.dll"),
            ("Microsoft.Bcl.AsyncInterfaces,", "Microsoft.Bcl.AsyncInterfaces.dll"),
            ("System.Numerics.Vectors,", "System.Numerics.Vectors.dll"),
            ("System.Drawing.Common,", "System.Drawing.Common.dll"),
            ("System.ValueTuple,", "System.ValueTuple.dll"),
            ("SixLabors.ImageSharp.Drawing,", "SixLabors.ImageSharp.Drawing.dll"),
            ("SixLabors.ImageSharp,", "SixLabors.ImageSharp.dll"),
            ("SixLabors.Fonts,", "SixLabors.Fonts.dll")
        };

        foreach (var assembly in assemblyList) {
            if (args.Name.StartsWith(assembly.Item1)) {
                var binaryPath = Path.Combine(Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location), assembly.Item2);
                return Assembly.LoadFile(binaryPath);
            }
        }

        return null;
    }
}