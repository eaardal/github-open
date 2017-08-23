namespace GitHubOpen
{
    class IssuesParam : IParam
    {
        public const string IssuesUrl = "issues";
        public const string IssueByIdUrl = "issue";

        public (bool ok, IParam parsedParam) Parse(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Equals("issues"))
                {
                    UrlFragment = IssuesUrl;
                    return (true, this);
                }

                if (arg.StartsWith("issue="))
                {
                    UrlFragment = $"{IssueByIdUrl}/{ParamUtil.SplitAndGetLast(arg, '=')}";
                    return (true, this);
                }

                if (arg.StartsWith("issue/"))
                {
                    UrlFragment = $"{IssueByIdUrl}/{ParamUtil.SplitAndGetLast(arg, '/')}";
                    return (true, this);
                }
            }

            return (false, null);
        }

        public string UrlFragment { get; private set; }
    }
}
