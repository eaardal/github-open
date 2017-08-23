using System;
using System.IO;

namespace GitHubOpen
{
    class GitHubOpenAppDataDirectory
    {
        public static string Path
        {
            get
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return System.IO.Path.Combine(appData, "GitHubOpen");
            }
        }

        public static string AliasesFilePath => System.IO.Path.Combine(Path, "aliases");

        public static void EnsureExists()
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }
    }
}
