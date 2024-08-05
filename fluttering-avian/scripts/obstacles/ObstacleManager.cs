namespace fluttering_avian.obstacles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using GodotTask;

public partial class ObstacleManager : Node
{
    [Export]
    private Godot.Collections.Dictionary<ObstacleType, PackedScene> obstacles;
    [Export]
    private float spawnOffset;
    [Export]
    private float obstacleMoveSpeed;
    [Export]
    private float cleaningMoveSpeedMultiplier;
    [Export]
    private Timer spawnTimer;

    private ObstacleSpawner spawner;
    private List<Obstacle> activeObstacles;
    private float moveSpeed;

    public event Action OnCharacterEnter;
    public event Action OnCharacterExit;
    public event Action OnCharacterCollision;

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
        moveSpeed = obstacleMoveSpeed;
        spawnTimer.Start();
    }

    public async Task StopSpawning()
    {
        spawnTimer.Stop();
        moveSpeed = obstacleMoveSpeed * cleaningMoveSpeedMultiplier;
        await GodotTask.WaitUntil(() => activeObstacles.Count == 0);
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
        RegisterObstacleHandlers(obstacle);
        AddChild(obstacle);
    }

    private void RegisterObstacleHandlers(Obstacle obstacle)
    {
        obstacle.OnCharacterEnter += OnCharacterEnterHandler;
        obstacle.OnCharacterExit += OnCharacterExitHandler;
        obstacle.OnCharacterCollision += OnCharacterCollisionHandler;
        obstacle.RegisterHandlers();
    }
    
    private void UnregisterObstacleHandlers(Obstacle obstacle)
    {
        obstacle.OnCharacterEnter -= OnCharacterEnterHandler;
        obstacle.OnCharacterExit -= OnCharacterExitHandler;
        obstacle.OnCharacterCollision -= OnCharacterCollisionHandler;
        obstacle.UnregisterHandlers();
    }

    private void OnCharacterEnterHandler()
    {
        OnCharacterEnter?.Invoke();
    }

    private void OnCharacterExitHandler()
    {
        OnCharacterExit?.Invoke();
    }

    private void OnCharacterCollisionHandler()
    {
        OnCharacterCollision?.Invoke();
    }

    private void TranslateObstacles(double delta)
    {
        foreach (var obstacle in activeObstacles.ToList())
        {
            obstacle.Translate(Vector3.Forward * moveSpeed * (float)delta);
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
        UnregisterObstacleHandlers(obstacle);
        activeObstacles.Remove(obstacle);
        obstacle.Free();
    }
}