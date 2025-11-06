using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BLL
{
    public static class VoskNativeLoader
    {
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            NativeLibrary.SetDllImportResolver(typeof(Vosk.Model).Assembly, DllImportResolver);
            _isInitialized = true;
        }

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // Map library names
            string actualLibName = libraryName switch
            {
                "vosk" => "libvosk.dll",
                "libvosk" => "libvosk.dll",
                _ => libraryName
            };

            string fullPath = Path.Combine(basePath, actualLibName);

            if (File.Exists(fullPath))
            {
                IntPtr handle = NativeLibrary.Load(fullPath);
                if (handle != IntPtr.Zero)
                    return handle;
            }

            // Fallback to default
            return IntPtr.Zero;
        }
    }
}