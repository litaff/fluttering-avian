namespace fluttering_avian.obstacles;

using System;
using Godot;

public partial class MovingObstacle : Obstacle
{
    [Export]
    private float moveSpeed;
    
    private Vector3 direction;
    private Vector3 offset;

    public override void _Ready()
    {
        base._Ready();
        var random = new Random();
        direction = random.Next(0, 2) == 0 ? Vector3.Down : Vector3.Up;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CalculateOffset(delta);
        Translate(offset);
    }

    private void CalculateOffset(double delta)
    {
        /*offset = direction * moveSpeed * (float)delta;
        
        if (Position.Y + offset.Y > MinSpawnHeight && Position.Y + offset.Y < MaxSpawnHeight) return;

        direction = -direction;
        offset = direction * moveSpeed * (float)delta;*/
    }
}