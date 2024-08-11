namespace fluttering_avian.obstacles;

using System;
using character_controller;
using Godot;
using Godot.Collections;
using Pooling;

public partial class Obstacle : Node3D, IPoolable
{
    [Export]
    public int MaxSpawnHeight { get; private set; }
    [Export]
    public int MinSpawnHeight { get; private set; }
    [Export]
    private Array<Area3D> solidZones;
    [Export]
    private Array<Area3D> gapZones;

    public event Action OnCharacterCollision;
    public event Action OnCharacterEnter;
    public event Action OnCharacterExit;
    public event Action<IPoolable> OnFreed;

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

    public virtual int GetRandomSpawnHeight()
    {
        var random = new Random();
        return random.Next(MinSpawnHeight, MaxSpawnHeight + 1);
    }

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