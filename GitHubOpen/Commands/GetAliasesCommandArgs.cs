using System.Linq;

namespace GitHubOpen.Commands
{
    internal class GetAliasesCommandArgs
    {
        public static bool Parse(string[] args)
        {
            if (args.Length != 1)
            {
                return false;
            }

            return args.Single() == "aliases" || args.Single() == "get-aliases";
        }
    }
}