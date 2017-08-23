using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHubOpen
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = new List<ICommand>
            {
                new SetAliasCommand(),
                new GetAliasesCommand(),
                new OpenCommand()
            };

            var command = commands.FirstOrDefault(cmd => cmd.HasValidArguments(args));

            if (command != null)
            {
                try
                {
                    command.Handle(args);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Yuck! An error! Please correct the below mistakes and try again.");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);

                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Your gibberish input did not match any known functionality. SAD.");
                Console.ResetColor();
            }
        }
    }
}
