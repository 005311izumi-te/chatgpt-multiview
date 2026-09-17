using ChatGPTMultiView;
using Xunit;

namespace ChatGPTMultiView.Tests;

public class TitleStoreTests
{
    [Fact]
    public void Load_WhenFileDoesNotExist_ReturnsDefaultTitles()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "titles.json");

        var titles = TitleStore.Load(path);

        Assert.Equal(["ChatGPT 1", "ChatGPT 2", "ChatGPT 3", "ChatGPT 4"], titles);
    }

    [Fact]
    public void SaveAndLoad_RoundTripsFourTitles()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var path = Path.Combine(directory, "titles.json");

        try
        {
            var expected = new[] { "設計", "実装", "調査", "レビュー" };

            TitleStore.Save(path, expected);
            var actual = TitleStore.Load(path);

            Assert.Equal(expected, actual);
        }
        finally
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }
}
