namespace GitHubOpen
{
    public interface IParam
    {
        (bool ok, IParam parsedParam) Parse(string[] args);
        string UrlFragment { get; }
    }
}