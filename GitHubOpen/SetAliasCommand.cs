using System;
using System.IO;
using System.Text;

namespace GitHubOpen
{
    class SetAliasCommand : ICommand
    {
        private SetAliasCommandArgs _parsedArgs;

        public bool HasValidArguments(string[] args)
        {
            (var ok, SetAliasCommandArgs aliasCommandArgs) = SetAliasCommandArgs.Parse(args);

            if (ok)
            {
                _parsedArgs = aliasCommandArgs;
            }

            return ok;
        }

        public void Handle(string[] args)
        {
            GitHubOpenAppDataDirectory.EnsureExists();

            var newAlias = $"{_parsedArgs.Alias},{_parsedArgs.GitRepositoryRootDirectory},{_parsedArgs.ParametersString}";
            File.AppendAllLines(GitHubOpenAppDataDirectory.AliasesFilePath, new []{ newAlias }, Encoding.UTF8);
        }
    }
}
