using System;
using System.Diagnostics;

namespace GitHubOpen.Commands
{
    public class OpenCommand : ICommand
    {
        private OpenCommandArgs _parsedArgs;

        public bool HasValidArguments(string[] args)
        {
            (var ok, OpenCommandArgs openCommandArgs) = OpenCommandArgs.Parse(args);

            if (ok)
            {
                _parsedArgs = openCommandArgs;
            }

            return ok;
        }

        public void Handle(string[] args)
        {
            WriteUrlToConsole();

            Process.Start(new ProcessStartInfo(_parsedArgs.GitHubUrl)
            {
                UseShellExecute = true,
            });
        }

        private void WriteUrlToConsole()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Opening ");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\"");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write((string) _parsedArgs.GitHubUrl);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\"");

            Console.ResetColor();
        }
    }
}