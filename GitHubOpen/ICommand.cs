namespace GitHubOpen
{
    internal interface ICommand
    {
        bool HasValidArguments(string[] args);
        void Handle(string[] args);
    }
}