namespace fluttering_avian.obstacles;

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ObstacleManager : Node
{
    [Export]
    private Godot.Collections.Dictionary<ObstacleType, PackedScene> obstacles;
    [Export]
    private float spawnOffset;
    [Export]
    private float obstacleMoveSpeed;
    [Export]
    private Timer spawnTimer;
    
    private ObstacleSpawner spawner;
    private List<Obstacle> activeObstacles;
    
    public override void _Ready()
    {
        activeObstacles = new List<Obstacle>();
        spawner = new ObstacleSpawner(
            new Dictionary<ObstacleType, PackedScene>(obstacles));
        spawnTimer.Timeout += SpawnObstacle;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        TranslateObstacles(delta);
    }

    public void StartSpawning()
    {
        spawnTimer.Start();
    }

    public void StopSpawning()
    {
        spawnTimer.Stop();
    }
    
    private void SpawnObstacle()
    {
        // TODO: Implement logic to swap between different obstacles.
        SpawnObstacle(ObstacleType.Regular);
    }
    
    private void SpawnObstacle(ObstacleType type)
    {
        var obstacle = spawner.GetObstacle(type);
        var initialPosition = new Vector3(0, obstacle.GetRandomSpawnHeight(), spawnOffset);
        obstacle.Position = initialPosition;
        activeObstacles.Add(obstacle);
        obstacle.RegisterHandlers();
        AddChild(obstacle);
    }

    private void TranslateObstacles(double delta)
    {
        foreach (var obstacle in activeObstacles.ToList())
        {
            obstacle.Translate(Vector3.Forward * obstacleMoveSpeed * (float)delta);
            if (!ReachedEnd(obstacle)) continue;
            FreeObstacle(obstacle);        
        }
    }

    private bool ReachedEnd(Node3D node)
    {
        return node.Position.Z <= -spawnOffset;
    }

    private void FreeObstacle(Obstacle obstacle)
    {
        RemoveChild(obstacle);
        obstacle.UnregisterHandlers();
        activeObstacles.Remove(obstacle);
        obstacle.Free();
    }
}