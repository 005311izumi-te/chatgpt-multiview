namespace ChatGPTMultiView;

public readonly record struct PanePlacement(
    int PaneIndex,
    int Row,
    int Column,
    int RowSpan,
    int ColumnSpan);

public static class LayoutPlanner
{
    public static IReadOnlyList<PanePlacement> ForPaneCount(int paneCount) =>
        paneCount switch
        {
            1 => [new(0, 0, 0, 2, 2)],
            2 => [
                new(0, 0, 0, 2, 1),
                new(1, 0, 1, 2, 1)
            ],
            3 => [
                new(0, 0, 0, 2, 1),
                new(1, 0, 1, 1, 1),
                new(2, 1, 1, 1, 1)
            ],
            4 => [
                new(0, 0, 0, 1, 1),
                new(1, 0, 1, 1, 1),
                new(2, 1, 0, 1, 1),
                new(3, 1, 1, 1, 1)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(paneCount))
        };
}
