using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace ChatGPTMultiView;

public partial class MainWindow : Window
{
    private static readonly Uri ChatGptUri = new("https://chatgpt.com/");
    private readonly WebView2[] _panes;
    private readonly FrameworkElement[] _paneContainers;
    private readonly TextBox[] _titleBoxes;
    private readonly string _titlesPath;

    public MainWindow()
    {
        InitializeComponent();

        _panes = [Pane1, Pane2, Pane3, Pane4];
        _paneContainers = [PaneContainer1, PaneContainer2, PaneContainer3, PaneContainer4];
        _titleBoxes = [Title1, Title2, Title3, Title4];
        _titlesPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ChatGPTMultiView",
            "titles.json");

        Loaded += OnLoaded;
        Closing += OnClosing;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            LoadTitles();
            ApplyLayout(4);
            await InitializeWebViewsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "WebView2 initialization failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void LoadTitles()
    {
        var titles = TitleStore.Load(_titlesPath);
        for (var index = 0; index < _titleBoxes.Length; index++)
        {
            _titleBoxes[index].Text = titles[index];
        }
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        TitleStore.Save(
            _titlesPath,
            _titleBoxes.Select(titleBox => titleBox.Text).ToArray());
    }

    private async Task InitializeWebViewsAsync()
    {
        var userDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ChatGPTMultiView",
            "WebView2");

        Directory.CreateDirectory(userDataFolder);

        var environment = await CoreWebView2Environment.CreateAsync(
            userDataFolder: userDataFolder);

        foreach (var pane in _panes)
        {
            await pane.EnsureCoreWebView2Async(environment);
            pane.CoreWebView2.Navigate(ChatGptUri.ToString());
        }
    }

    private void PaneCountButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string tag } &&
            int.TryParse(tag, out var paneCount))
        {
            ApplyLayout(paneCount);
        }
    }

    private void ApplyLayout(int paneCount)
    {
        foreach (var container in _paneContainers)
        {
            container.Visibility = Visibility.Collapsed;
            Grid.SetRow(container, 0);
            Grid.SetColumn(container, 0);
            Grid.SetRowSpan(container, 1);
            Grid.SetColumnSpan(container, 1);
        }

        foreach (var placement in LayoutPlanner.ForPaneCount(paneCount))
        {
            var container = _paneContainers[placement.PaneIndex];
            Grid.SetRow(container, placement.Row);
            Grid.SetColumn(container, placement.Column);
            Grid.SetRowSpan(container, placement.RowSpan);
            Grid.SetColumnSpan(container, placement.ColumnSpan);
            container.Visibility = Visibility.Visible;
        }
    }
}
