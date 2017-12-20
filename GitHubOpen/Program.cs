using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHubOpen
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commandHandler = new CommandHandler();

            if (IsRepl(args))
            {
                var input = string.Empty;
                while (input != "exit")
                {
                    Console.WriteLine("Enter a command:");
                    input = Console.ReadLine();

                    commandHandler.FindAndExecuteCommand(input.Split(' '));
                }
            }
            else
            {
                commandHandler.FindAndExecuteCommand(args);
            }
        }

        private static bool IsRepl(IEnumerable<string> args)
        {
            return new[] {"-r", "--repl"}.Any(args.Contains);
        }
    }
}
