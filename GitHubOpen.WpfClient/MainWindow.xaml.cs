using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Octokit;

namespace GitHubOpen.WpfClient
{
    public partial class MainWindow
    {
        private static readonly string GitHubEnterpriseUrl = ConfigurationManager.AppSettings["GitHubEnterprise.Url"];

        private readonly GitHubClient _gitHubClient;
        private int _searchResultsTraversingIndex = 0;
        private int _nrOfSearchResults = 0;
        private List<Repository> _repositories = new List<Repository>();

        public MainWindow()
        {
            InitializeComponent();
            Activated += OnActivated;
            
            if (GitHubEnterpriseUrl == null)
            {
                _gitHubClient = new GitHubClient(new ProductHeaderValue("github-open"));
            }
            else
            {
                var ghe = new Uri(GitHubEnterpriseUrl);
                _gitHubClient = new GitHubClient(new ProductHeaderValue("github-open"), ghe);
            }

            KeyUp += OnKeyUp;
        }

        private void ColorHiglightedItem(Border selectedItem)
        {
            foreach (Border border in SearchResultsContainer.Items)
            {
                border.Background = border.Tag == selectedItem.Tag
                    ? new SolidColorBrush(Colors.PowderBlue)
                    : new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private async void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (_nrOfSearchResults == 0)
                {
                    return;
                }

                if (_searchResultsTraversingIndex + 1 < _nrOfSearchResults)
                {
                    _searchResultsTraversingIndex++;

                    var selectedItem = (Border)SearchResultsContainer.Items[_searchResultsTraversingIndex];
                    ColorHiglightedItem(selectedItem);
                }
            }

            if (e.Key == Key.Up)
            {
                if (_nrOfSearchResults == 0)
                {
                    return;
                }

                if (_searchResultsTraversingIndex - 1 > -1)
                {
                    _searchResultsTraversingIndex--;

                    var selectedItem = (Border)SearchResultsContainer.Items[_searchResultsTraversingIndex];
                    ColorHiglightedItem(selectedItem);
                }
            }

            if (KeyboardFacade.IsLeftCtrlDown() && e.Key == Key.O)
            {
                if (_searchResultsTraversingIndex > -1)
                {
                    var selectedRepo = _repositories.ElementAt(_searchResultsTraversingIndex);
                    Process.Start(new ProcessStartInfo(selectedRepo.HtmlUrl)
                    {
                        UseShellExecute = true,
                    });
                }
            }

            if (KeyboardFacade.IsLeftCtrlDown() && e.Key == Key.P)
            {
                if (_searchResultsTraversingIndex > -1)
                {
                    var selectedRepo = _repositories.ElementAt(_searchResultsTraversingIndex);
                    Process.Start(new ProcessStartInfo($"{selectedRepo.HtmlUrl}/pulls")
                    {
                        UseShellExecute = true,
                    });
                }
            }

            if (KeyboardFacade.IsLeftCtrlDown() && e.Key == Key.R)
            {
                if (_searchResultsTraversingIndex > -1)
                {
                    var selectedRepo = _repositories.ElementAt(_searchResultsTraversingIndex);
                    var readme = await _gitHubClient.Repository.Content.GetReadmeHtml(selectedRepo.Id);
                    var githubCss = "https://cdnjs.cloudflare.com/ajax/libs/github-markdown-css/2.9.0/github-markdown.css";

                    var html = new StringBuilder();
                    html.AppendLine("<html>");
                    html.AppendLine("<head>");
                    html.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
                    html.AppendLine($"<link type=\"text/css\" rel=\"stylesheet\" href=\"{githubCss}\">");
                    //html.Append("<style></style>");
                    html.Append("<style>\r\n\t.markdown-body {\r\n\t\tbox-sizing: border-box;\r\n\t\tmin-width: 200px;\r\n\t\tmax-width: 980px;\r\n\t\tmargin: 0 auto;\r\n\t\tpadding: 45px;\r\n\t}\r\n\r\n\t@media (max-width: 767px) {\r\n\t\t.markdown-body {\r\n\t\t\tpadding: 15px;\r\n\t\t}\r\n\t}\r\n</style>");

                    html.AppendLine("</head>");
                    html.AppendLine("<body>");
                    //html.AppendLine("<article class=\"markdown-body\">");
                    html.AppendLine(readme);
                    //html.AppendLine("</article>");
                    html.AppendLine("</body>");
                    html.AppendLine("</html>");
                    
                    
                    Browser.NavigateToString(html.ToString());
                    Browser.Visibility = Visibility.Visible;
                    Width = Width + 600;
                }
            }
        }

        private void OnActivated(object sender, EventArgs eventArgs)
        {
            TxtSearch.Focus();
        }

        private async void TxtSearch_OnKeyUp(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox) sender;

            if (e.Key == Key.Enter)
            {
                await Search(textBox.Text);
            }
        }

        private async Task Search(string query)
        {
            SearchResultsContainer.Items.Clear();
            _repositories.Clear();

            var searchRepositoriesRequest = new SearchRepositoriesRequest(query);

            var result = await _gitHubClient.Search.SearchRepo(searchRepositoriesRequest);
            _nrOfSearchResults = result.TotalCount;
            
            foreach (var repository in result.Items)
            {
                WriteSearchResult(repository);
                _repositories.Add(repository);
            }

            ColorHiglightedItem((Border)SearchResultsContainer.Items[_searchResultsTraversingIndex]);
        }

        private void WriteSearchResult(Repository repo)
        {
            var img = new Image
            {
                Source = new BitmapImage(new Uri(repo.Owner.AvatarUrl)),
                Height = 30,
                Width = 30,
                Margin = new Thickness(5)
            };
            var repoOwnerTextBlock = new TextBlock
            {
                Text = $"{repo.Owner.Login}/",
                FontFamily = new FontFamily("Consolas"),
                VerticalAlignment = VerticalAlignment.Center
            };
            var repoNameTextBlock = new TextBlock
            {
                Text = repo.Name,
                FontFamily = new FontFamily("Consolas"),
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                VerticalAlignment = VerticalAlignment.Center
            };
            var border = new Border
            {
                Tag = repo.Url,
                Margin = new Thickness(2),
                Padding = new Thickness(2),
                Background = new SolidColorBrush(Colors.AliceBlue)
            };
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            border.Child = stackPanel;
            stackPanel.Children.Add(img);
            stackPanel.Children.Add(repoOwnerTextBlock);
            stackPanel.Children.Add(repoNameTextBlock);
            
            //var openButton = new Button
            //{
            //    Content = "open"
            //};
            //var prButton = new Button
            //{
            //    Content = "prs"
            //};
            //stackPanel.Children.Add(openButton);
            //stackPanel.Children.Add(prButton);

            SearchResultsContainer.Items.Add(border);
        }
    }
}
