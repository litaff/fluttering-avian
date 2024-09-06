namespace fluttering_avian.obstacles;

using System;
using System.Collections.Generic;
using System.Linq;
using character_controller;
using Godot;
using Pooling;

public partial class Obstacle : Node3D, IPoolable
{
    protected const int SOLID_MARGIN = 20;
    
    private List<Area3D> solidZones;
    private List<Area3D> gapZones;
    
    public event Action<Obstacle> OnCharacterCollision;
    public event Action<Obstacle> OnCharacterEnter;
    public event Action<Obstacle> OnCharacterExit;
    public event Action<IPoolable> OnFreed;

    public virtual void Initialize(ObstacleData data, float gameHeight)
    {
        gapZones ??= SpawnGaps(data.GapData, gameHeight);
        solidZones ??= SpawnSolids(data.SolidData, data.GapData.Height, gapZones);
    }
    
    public void RegisterHandlers()
    {
        foreach (var solidZone in solidZones)
        {
            solidZone.BodyEntered += OnSolidZoneEnteredHandler;
        }

        foreach (var gapZone in gapZones)
        {
            gapZone.BodyEntered += OnGapZoneEnteredHandler;
            gapZone.BodyExited += OnGapZoneExitedHandler;
        }
    }

    public void UnregisterHandlers()
    {
        foreach (var solidZone in solidZones)
        {
            solidZone.BodyEntered -= OnSolidZoneEnteredHandler;
        }

        foreach (var gapZone in gapZones)
        {
            gapZone.BodyEntered -= OnGapZoneEnteredHandler;
            gapZone.BodyExited -= OnGapZoneExitedHandler;
        }
    }

    public new void Free()
    {
        OnFreed?.Invoke(this);
    }

    private void OnSolidZoneEnteredHandler(Node3D body)
    {
        if (body is not CharacterController) return;
        OnCharacterCollision?.Invoke(this);
    }

    private void OnGapZoneEnteredHandler(Node3D body)
    {
        if (body is not CharacterController) return;
        OnCharacterEnter?.Invoke(this);
    }

    private void OnGapZoneExitedHandler(Node3D body)
    {
        if (body is not CharacterController) return;
        OnCharacterExit?.Invoke(this);
    }

    private List<Area3D> SpawnSolids(SolidObstacleData solidData, float gapHeight, List<Area3D> gaps)
    {
        var solids = new List<Area3D>();
        
        //TODO: THIS DO THIS
        var bottomSolid = GetArea(gaps[0].Position.Y, solidData.CollisionData);
        
        
        for (var i = 0; i < gaps.Count - 1; i++)
        {
            var lowGap = gaps[i];
            var highGap = gaps[i + 1];
            var solid = GetArea(lowGap.Position.DistanceTo(highGap.Position) - gapHeight, solidData.CollisionData);
            
            var spawnHeight = (lowGap.Position.Y + highGap.Position.Y) / 2;
            
            solid.Position = new Vector3(0, spawnHeight, 0);
            
            var meshInstance = new MeshInstance3D();
            meshInstance.Mesh = solidData.Mesh;
            meshInstance.MaterialOverride = solidData.Material;
            solid.AddChild(meshInstance);

            AddChild(solid);
            solids.Add(solid);
        }
        
        return solids;
    }
    
    private List<Area3D> SpawnGaps(GapObstacleData gapData, float gameHeight)
    {
        var gaps = new List<Area3D>();
        var random = new Random();
        var gameHeightPerGap = gameHeight / gapData.Amount;
        
        ValidateHeightData(gapData.Height, gameHeightPerGap);

        for (var i = 0; i < gapData.Amount; i++)
        {
            var gap = GetArea(gapData.Height, gapData.CollisionData);

            var incrementOffset = gameHeightPerGap * i;
            var randomHeightForGap = (gameHeightPerGap - gapData.Height) * (float)random.NextDouble() + gapData.Height / 2;
            var gameOffset = gameHeight / 2;
            var spawnHeight = incrementOffset + randomHeightForGap - gameOffset;
            
            gap.Position = new Vector3(0, spawnHeight, 0);

            AddChild(gap);
            gaps.Add(gap);
        }
        
        return gaps;
    }

    private static Area3D GetArea(float height, ObstacleCollisionData collisionData)
    {
        var area = new Area3D();
        area.Monitoring = true;
        area.CollisionLayer = collisionData.CollisionLayer;
        area.CollisionMask = collisionData.CollisionMask;
            
        var shape = new BoxShape3D();
        shape.Size = new Vector3(1, height, 1);
        var ownerId = area.CreateShapeOwner(new GodotObject());
        area.ShapeOwnerAddShape(ownerId, shape);
        return area;
    }

    /// <summary>
    /// Throws an exception if the gap height is too large for the amount of gaps to spawn.
    /// </summary>
    private void ValidateHeightData(float height, float gameHeightPerGap)
    {
        if (gameHeightPerGap - height < 0)
            throw new ArgumentException("[Obstacle] Gap height is too large for the amount of gaps to spawn. ");
    }
}