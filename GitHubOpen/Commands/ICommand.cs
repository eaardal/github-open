namespace GitHubOpen.Commands
{
    internal interface ICommand
    {
        bool HasValidArguments(string[] args);
        void Handle(string[] args);
    }
}