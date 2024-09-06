namespace fluttering_avian.obstacles;

using Godot;

[GlobalClass]
public partial class SolidObstacleData : Resource
{
    [Export(PropertyHint.Layers3DPhysics)]
    public ObstacleCollisionData CollisionData { get; private set; }
    [Export]
    public Mesh Mesh { get; private set; }
    [Export]
    public Material Material { get; private set; }
}