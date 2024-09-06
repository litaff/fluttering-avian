namespace fluttering_avian.obstacles;

using Godot;

[GlobalClass]
public partial class ObstacleCollisionData : Resource
{
    [Export(PropertyHint.Layers3DPhysics)]
    public uint CollisionLayer { get; private set; }
    [Export(PropertyHint.Layers3DPhysics)]
    public uint CollisionMask { get; private set; }
}