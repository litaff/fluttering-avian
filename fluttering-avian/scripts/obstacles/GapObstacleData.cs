namespace fluttering_avian.obstacles;

using Godot;

[GlobalClass]
public partial class GapObstacleData : Resource
{
    [Export]
    public float Height { get; private set; }
    [Export]
    public int Amount { get; private set; }
    [Export(PropertyHint.Layers3DPhysics)]
    public ObstacleCollisionData CollisionData { get; private set; }
}