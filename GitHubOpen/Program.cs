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
                new OpenCommand()
            };

            foreach (var command in commands.Where(command => command.HasValidArguments(args)))
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
        }
    }
}
