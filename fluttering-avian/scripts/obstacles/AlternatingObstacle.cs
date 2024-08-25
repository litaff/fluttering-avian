namespace fluttering_avian.obstacles;

using System;
using Godot;
using Godot.Collections;

public partial class AlternatingObstacle : Obstacle
{
    [Export]
    private Array<Area3D> blockers;

    private Random random;

    public override void _Ready()
    {
        base._Ready();
        random = new Random();
        Randomize();
    }

    public override int GetRandomSpawnHeight()
    {
        return base.GetRandomSpawnHeight();
    }

    /// <summary>
    /// Alternates the blockers on this obstacle.
    /// </summary>
    /// <param name="obstacle">Obstacle which caused this call.</param>
    public void Alternate(Obstacle obstacle)
    {
        if (obstacle == this) return;
        
        Randomize();
    }

    private void Randomize()
    {
        var availableBlockers = RemoveAtRandom();

        foreach (var blocker in availableBlockers)
        {
            if (blocker.IsInsideTree()) continue;
            AddChild(blocker);
        }
    }

    private Array<Area3D> RemoveAtRandom()
    {
        var amountToDeactivate = random.Next(1, blockers.Count - 1);
        var availableBlockers = new Array<Area3D>(blockers);
        while (amountToDeactivate > 0)
        {
            var blockerToDeactivate = availableBlockers.PickRandom();
            availableBlockers.Remove(blockerToDeactivate);
            amountToDeactivate--;
            
            if (!blockerToDeactivate.IsInsideTree()) continue;
            RemoveChild(blockerToDeactivate);
        }

        return availableBlockers;
    }
}