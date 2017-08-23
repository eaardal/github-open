namespace GitHubOpen
{
    public class PullRequestParam : IParam
    {
        private const string PullRequestsUrl = "pulls";
        private const string PullRequestByIdUrl = "pull";

        public (bool ok, IParam parsedParam) Parse(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Equals("prs") || arg.Equals("pull-requests") || args.Equals("pulls"))
                {
                    UrlFragment = PullRequestsUrl;
                    return (true, this);
                }

                if (arg.StartsWith("pr=") || arg.StartsWith("pull="))
                {
                    UrlFragment = $"{PullRequestByIdUrl}/{ParamUtil.SplitAndGetLast(arg, '=')}";
                    return (true, this);
                }

                if (arg.StartsWith("pr/") || arg.StartsWith("pull/"))
                {
                    UrlFragment = $"{PullRequestByIdUrl}/{ParamUtil.SplitAndGetLast(arg, '/')}";
                    return (true, this);
                }
            }

            return (false, null);
        }

        public string UrlFragment { get; private set; }
    }
}