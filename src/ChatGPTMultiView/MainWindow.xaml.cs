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

    public MainWindow()
    {
        InitializeComponent();
        _panes = [Pane1, Pane2, Pane3, Pane4];
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
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
        foreach (var pane in _panes)
        {
            pane.Visibility = Visibility.Collapsed;
            Grid.SetRow(pane, 0);
            Grid.SetColumn(pane, 0);
            Grid.SetRowSpan(pane, 1);
            Grid.SetColumnSpan(pane, 1);
        }

        foreach (var placement in LayoutPlanner.ForPaneCount(paneCount))
        {
            var pane = _panes[placement.PaneIndex];
            Grid.SetRow(pane, placement.Row);
            Grid.SetColumn(pane, placement.Column);
            Grid.SetRowSpan(pane, placement.RowSpan);
            Grid.SetColumnSpan(pane, placement.ColumnSpan);
            pane.Visibility = Visibility.Visible;
        }
    }
}
