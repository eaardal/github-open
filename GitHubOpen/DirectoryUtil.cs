using System.IO;
using System.Linq;

namespace GitHubOpen
{
    public class DirectoryUtil
    {
        public static bool IsExistingGitRepositoryRootDirectory(string directory)
        {
            return Directory.Exists(directory) && HasDotGitSubDirectory(directory);
        }

        private static bool HasDotGitSubDirectory(string directory)
        {
            var subDirectories = new DirectoryInfo(directory).GetDirectories()
                .Where(x => (x.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden).ToArray();

            return subDirectories.Any(subDir => subDir.Name.Equals(".git"));
        }
    }
}
