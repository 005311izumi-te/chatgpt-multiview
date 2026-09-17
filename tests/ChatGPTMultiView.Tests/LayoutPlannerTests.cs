using ChatGPTMultiView;

namespace ChatGPTMultiView.Tests;

public class LayoutPlannerTests
{
    [Fact]
    public void OnePane_FillsTheGrid()
    {
        var plan = LayoutPlanner.ForPaneCount(1);

        Assert.Equal(
            [new PanePlacement(0, 0, 0, 2, 2)],
            plan);
    }

    [Fact]
    public void TwoPanes_AreSideBySide()
    {
        var plan = LayoutPlanner.ForPaneCount(2);

        Assert.Equal(
            [
                new PanePlacement(0, 0, 0, 2, 1),
                new PanePlacement(1, 0, 1, 2, 1)
            ],
            plan);
    }

    [Fact]
    public void ThreePanes_UseLargeLeftAndStackedRight()
    {
        var plan = LayoutPlanner.ForPaneCount(3);

        Assert.Equal(
            [
                new PanePlacement(0, 0, 0, 2, 1),
                new PanePlacement(1, 0, 1, 1, 1),
                new PanePlacement(2, 1, 1, 1, 1)
            ],
            plan);
    }

    [Fact]
    public void FourPanes_UseTwoByTwoGrid()
    {
        var plan = LayoutPlanner.ForPaneCount(4);

        Assert.Equal(
            [
                new PanePlacement(0, 0, 0, 1, 1),
                new PanePlacement(1, 0, 1, 1, 1),
                new PanePlacement(2, 1, 0, 1, 1),
                new PanePlacement(3, 1, 1, 1, 1)
            ],
            plan);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void InvalidPaneCount_Throws(int paneCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => LayoutPlanner.ForPaneCount(paneCount));
    }
}
