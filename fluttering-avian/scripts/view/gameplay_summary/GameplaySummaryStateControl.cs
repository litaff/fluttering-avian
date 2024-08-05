namespace fluttering_avian.view.gameplay_summary;

using Godot;

public partial class GameplaySummaryStateControl : Control
{
    [Export]
    public Label ScoreLabel { get; private set; }
    [Export]
    public Button RestartButton { get; private set; }
}