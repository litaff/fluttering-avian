namespace fluttering_avian.obstacles;

using System;
using character_controller;
using Godot;
using Godot.Collections;
using Pooling;

public partial class Obstacle : Node3D, IPoolable
{
    [Export]
    private int maxSpawnHeight;
    [Export]
    private int minSpawnHeight;
    [Export]
    private Array<Area3D> solidZones;
    [Export]
    private Array<Area3D> gapZones;

    public event Action OnCharacterCollision;
    public event Action OnCharacterEnter;
    public event Action OnCharacterExit;
    public event Action<IPoolable> OnFreed;

    /// <summary>
    /// Initializes the obstacle with the given parameters. Use if setting those parameters is not possible in the editor.
    /// </summary>
    /// <param name="maxSpawnHeight">Maximum for positive Y offset.</param>
    /// <param name="minSpawnHeight">Maximum for negative Y offset.</param>
    /// <param name="solidZones">Area3D which will detect <see cref="OnCharacterCollision"/>.</param>
    /// <param name="gapZones">Area3D which will detect <see cref="OnCharacterEnter"/> and <see cref="OnCharacterExit"/>.</param>
    public void Initialize(int maxSpawnHeight, int minSpawnHeight, Array<Area3D> solidZones, Array<Area3D> gapZones)
    {
        this.maxSpawnHeight = maxSpawnHeight;
        this.minSpawnHeight = minSpawnHeight;
        this.solidZones = solidZones;
        this.gapZones = gapZones;
    }
    
    /// <summary>
    /// Registers event handlers to Area3D nodes.
    /// </summary>
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

    /// <summary>
    /// Unregisters event handlers from Area3D nodes.
    /// </summary>
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

    /// <summary>
    /// Returns a random height within the specified range between <see cref="minSpawnHeight"/> and <see cref="maxSpawnHeight"/>.
    /// </summary>
    public virtual int GetRandomSpawnHeight(Random random)
    {
        return random.Next(minSpawnHeight, maxSpawnHeight + 1);
    }

    /// <summary>
    /// Frees the obstacle and invokes <see cref="OnFreed"/>.
    /// </summary>
    public new void Free()
    {
        OnFreed?.Invoke(this);
    }
    
    private void OnSolidZoneEnteredHandler(Node3D body)
    {
        if (body is not CharacterController) return;
        OnCharacterCollision?.Invoke();
    }

    private void OnGapZoneEnteredHandler(Node3D body)
    {
        if (body is not CharacterController) return;
        OnCharacterEnter?.Invoke();
    }

    private void OnGapZoneExitedHandler(Node3D body)
    {
        if (body is not CharacterController) return;
        OnCharacterExit?.Invoke();
    }
}