using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Pokedex.Pokerole.Extensions
{
    internal static class AssemblyExtensions
    {
        private static Dictionary<Assembly, string[]> FilesCache { get; set; } = new Dictionary<Assembly, string[]>();
        
        public static string GetFileContentsFromAssembly(this Assembly assembly, string fileName)
        {
            var fileStream = GetFileStream(assembly, fileName);
            using var streamReader = new StreamReader(fileStream);
            return streamReader.ReadToEnd();
        }

        public static Stream GetFileStream(this Assembly assembly, string fileName)
        {
            if (!FilesCache.ContainsKey(assembly))
            {
                FilesCache.Add(assembly, assembly.GetManifestResourceNames());
            }

            var fullFileName = FilesCache[assembly].Single(i => i.Contains(fileName));
            var fileStream = assembly.GetManifestResourceStream(fullFileName);
            return fileStream;
        }
    }
}