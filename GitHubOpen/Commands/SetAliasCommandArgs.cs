using System.Linq;

namespace GitHubOpen.Commands
{
    internal class SetAliasCommandArgs
    {
        public string Alias { get; private set; }
        public string GitRepositoryRootDirectory { get; private set; }
        public string[] Parameters { get; private set; }
        public string ParametersString { get; private set; }

        public static (bool ok, SetAliasCommandArgs aliasCommandArgs) Parse(string[] args)
        {
            /*  1st arg (req) should be "set-alias" command
             *  2nd arg (req) should be the alias to use
             *  3rd arg (req) should be a valid path to a git repository directory
             *  4th arg (opt) should be a valid parameter
             */

            if (args.Length < 3 || args.Length > 4)
            {
                return (false, null);
            }

            var aliasCommand = args[0];
            var alias = args[1];
            var gitRepositoryRootDirectory = args[2];
            
            if (aliasCommand != "set-alias")
            {
                return (false, null);
            }

            if (!DirectoryUtil.IsExistingGitRepositoryRootDirectory(gitRepositoryRootDirectory))
            {
                return (false, null);
            }

            var parameters = args.Length > 3 ? args.Skip(3).Take(args.Length - 3).ToArray() : new string[0];
            var parametersString = parameters.Any() ? parameters.Aggregate((param, str) => str + ";" + param) : null;

            var aliasCommandArgs = new SetAliasCommandArgs
            {
                Alias = alias,
                GitRepositoryRootDirectory = gitRepositoryRootDirectory,
                Parameters = parameters,
                ParametersString = parametersString
            };
            
            return (true, aliasCommandArgs);
        }
    }
}