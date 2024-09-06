namespace fluttering_avian.obstacles;

using Godot;

[GlobalClass]
public partial class ObstacleData : Resource
{
    [Export]
    public GapObstacleData GapData { get; private set; }
    [Export]
    public SolidObstacleData SolidData { get; private set; }
}