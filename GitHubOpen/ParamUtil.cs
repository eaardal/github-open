using System.Linq;

namespace GitHubOpen
{
    class ParamUtil
    {
        public static string SplitAndGetLast(string str, char divider)
        {
            return str.Split(divider).Last().Trim();
        }
    }
}
