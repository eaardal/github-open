using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GitHubOpen.Commands
{
    class GetAliasesCommand : ICommand
    {
        public bool HasValidArguments(string[] args)
        {
            return GetAliasesCommandArgs.Parse(args);
        }

        public void Handle(string[] args)
        {
            GitHubOpenAppDataDirectory.EnsureExists();

            var lines = File.ReadAllLines(GitHubOpenAppDataDirectory.AliasesFilePath, Encoding.UTF8);
            var aliases = Alias.ParseFromFile(lines);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Aliases ({GitHubOpenAppDataDirectory.AliasesFilePath}):");

            foreach (var alias in aliases)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("  Key: ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{alias.Key}{Environment.NewLine}");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("  Path: ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{alias.DirectoryPath}{Environment.NewLine}");
                
                if (alias.Parameters.Any())
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  Parameters: ");

                    foreach (var parameter in alias.Parameters)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{parameter}{Environment.NewLine}");
                    }
                }

                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
