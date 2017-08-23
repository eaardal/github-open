using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GitHubOpen
{
    public class OpenCommandArgs
    {
        public string GitHubUrl { get; private set; }

        public static (bool ok, OpenCommandArgs parsedArgs) Parse(string[] args)
        {
            var openCommandArgs = new OpenCommandArgs();
            
            (var foundAlias, Alias alias) = TryResolveKnownAlias(args);

            var directory = foundAlias
                ? alias.DirectoryPath
                : ResolveGitRepositoryRootDirectory(args);

            var gitRepositoryUrl = ResolveGitRepositoryUrl(directory);

            var urlParams = new List<IParam>
            {
                new IssuesParam(),
                new PullRequestParam()
            };

            openCommandArgs.GitHubUrl = gitRepositoryUrl;
            
            foreach (var param in urlParams)
            {
                (var ok, var parsedParam) = foundAlias ? param.Parse(alias.Parameters) : param.Parse(args);

                if (ok)
                {
                    openCommandArgs.GitHubUrl = $"{gitRepositoryUrl}/{parsedParam.UrlFragment}";
                }
            }

            if (string.IsNullOrEmpty(openCommandArgs.GitHubUrl))
            {
                return (false, null);
            }

            return (true, openCommandArgs);
        }
        
        private static (bool foundAlias, Alias alias) TryResolveKnownAlias(string[] args)
        {
            GitHubOpenAppDataDirectory.EnsureExists();

            if (File.Exists(GitHubOpenAppDataDirectory.AliasesFilePath))
            {
                var aliasesLines = File.ReadAllLines(GitHubOpenAppDataDirectory.AliasesFilePath);

                var aliases = Alias.ParseFromFile(aliasesLines);

                var alias = aliases.FirstOrDefault(a => args.Contains(a.Key));

                return (alias != null, alias);
            }

            return (false, null);
        }

        private static string ResolveGitRepositoryUrl(string directory)
        {
            var gitRepositoryConfigFilePath = Path.Combine(directory, ".git", "config");

            var gitRepositoryConfigFile = File.ReadAllLines(gitRepositoryConfigFilePath, Encoding.UTF8);

            var urlLine = gitRepositoryConfigFile.SingleOrDefault(line => line.Contains("url = "));

            if (urlLine != null)
            {
                var url = urlLine.Split('=').Last().Trim();

                return StripDotGitUrlEnding(url);
            }
            
            throw new Exception($"No remote git repository url found in \"{gitRepositoryConfigFilePath}\". Maybe you haven't added a remote to your repository yet?");
        }

        private static string StripDotGitUrlEnding(string url)
        {
            return url.Substring(0, url.Length - 4); // -4 to remove the last ".git"-part of the url
        }

        private static string ResolveGitRepositoryRootDirectory(string[] args)
        {
            var gitRepositoryRootDirectory = string.Empty;

            foreach (var arg in args)
            {
                if (DirectoryUtil.IsExistingGitRepositoryRootDirectory(arg))
                {
                    gitRepositoryRootDirectory = arg;
                }
            }

            if (string.IsNullOrEmpty(gitRepositoryRootDirectory))
            {
                var currentDirectory = Environment.CurrentDirectory;

                if (DirectoryUtil.IsExistingGitRepositoryRootDirectory(currentDirectory))
                {
                    gitRepositoryRootDirectory = currentDirectory;
                }
            }

            if (string.IsNullOrEmpty(gitRepositoryRootDirectory))
            {
                throw new Exception(
                    "No Git repository root directory provided. Try specifying the absolute or relative path to the root directory of a Git repository.");
            }

            return gitRepositoryRootDirectory;
        }
    }
}