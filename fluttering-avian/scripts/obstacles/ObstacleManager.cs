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

    public event Action<Obstacle> OnCharacterEnter;
    public event Action<Obstacle> OnCharacterExit;
    public event Action<Obstacle> OnCharacterCollision;

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
        // TEMP, this should prolly be weighted.
        var random = new Random();
        var type = random.Next(0, 3);
        GD.Print($"Spawning {type}");
        SpawnObstacle((ObstacleType)type);
    }
    
    private void SpawnObstacle(ObstacleType type)
    {
        var obstacle = spawner.GetObstacle(type);
        var initialPosition = new Vector3(0, obstacle.GetRandomSpawnHeight(), spawnOffset);
        obstacle.Position = initialPosition;
        activeObstacles.Add(obstacle);
        RegisterObstacleHandlers(obstacle);
        AddChild(obstacle);
        InitializeObstacle(obstacle);
    }

    private void InitializeObstacle(Obstacle obstacle)
    {
        switch (obstacle)
        {
            case AlternatingObstacle alternatingObstacle:
                OnCharacterEnter += alternatingObstacle.Alternate;
                OnCharacterExit += alternatingObstacle.Alternate;
                break;
        }
    }

    private void DisposeObstacle(Obstacle obstacle)
    {
        switch (obstacle)
        {
            case AlternatingObstacle alternatingObstacle:
                OnCharacterEnter -= alternatingObstacle.Alternate;
                OnCharacterExit -= alternatingObstacle.Alternate;
                break;
        }
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

    private void OnCharacterEnterHandler(Obstacle obstacle)
    {
        OnCharacterEnter?.Invoke(obstacle);
    }

    private void OnCharacterExitHandler(Obstacle obstacle)
    {
        OnCharacterExit?.Invoke(obstacle);
    }

    private void OnCharacterCollisionHandler(Obstacle obstacle)
    {
        OnCharacterCollision?.Invoke(obstacle);
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
        DisposeObstacle(obstacle);
        RemoveChild(obstacle);
        UnregisterObstacleHandlers(obstacle);
        activeObstacles.Remove(obstacle);
        obstacle.Free();
    }
}