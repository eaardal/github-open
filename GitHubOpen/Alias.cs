using System.Collections.Generic;
using System.Linq;

namespace GitHubOpen
{
    public class Alias
    {
        public string Key { get; }
        public string DirectoryPath  { get; }
        public string[] Parameters { get; private set; }

        private Alias(string key, string directoryPath)
        {
            Key = key;
            DirectoryPath = directoryPath;
            Parameters = new string[0];
        }

        public void SetParameters(string[] parameters)
        {
            Parameters = parameters;
        }
        
        public static IReadOnlyList<Alias> ParseFromFile(string[] lines)
        {
            return lines.Select(line =>
            {
                var parts = line.Split(',');
                var aliasKey = parts[0];
                var directory = parts[1];

                var alias = new Alias(aliasKey, directory);

                if (parts.Length == 3)
                {
                    var parameters = parts[2].Split(';');
                    alias.SetParameters(parameters);
                }

                return alias;

            }).ToList();
        }
    }
}