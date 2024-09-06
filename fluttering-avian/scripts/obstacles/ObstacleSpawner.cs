namespace fluttering_avian.obstacles;

using System;
using System.Collections.Generic;

[Serializable]
public class ObstacleSpawner
{
    private Dictionary<ObstacleType, ObstaclePooling> obstaclePoolings;

    public ObstacleSpawner(Dictionary<ObstacleType, ObstacleData> obstacles, float gameHeight)
    {
        obstaclePoolings = new Dictionary<ObstacleType, ObstaclePooling>();
        foreach (var obstacle in obstacles)
        {
            obstaclePoolings.Add(obstacle.Key, new ObstaclePooling(obstacle.Value, gameHeight));
        }
    }

    public Obstacle GetObstacle(ObstacleType type)
    {
        var obstacle = obstaclePoolings[type].GetPoolable();
        
        return obstacle;
    }
}