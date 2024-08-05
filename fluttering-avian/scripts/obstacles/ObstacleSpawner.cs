namespace fluttering_avian.obstacles;

using System;
using System.Collections.Generic;
using Godot;

[Serializable]
public class ObstacleSpawner
{
    private Dictionary<ObstacleType, ObstaclePooling> obstaclePoolings;

    public ObstacleSpawner(Dictionary<ObstacleType, PackedScene> obstacles)
    {
        obstaclePoolings = new Dictionary<ObstacleType, ObstaclePooling>();
        foreach (var obstacle in obstacles)
        {
            obstaclePoolings.Add(obstacle.Key, new ObstaclePooling(obstacle.Value));
        }
    }

    public Obstacle GetObstacle(ObstacleType type)
    {
        var obstacle = obstaclePoolings[type].GetPoolable();
        
        return obstacle;
    }
}