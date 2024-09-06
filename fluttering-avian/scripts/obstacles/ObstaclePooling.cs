namespace fluttering_avian.obstacles;

using System.Linq;
using Godot;
using Pooling;

public class ObstaclePooling : Pooling<Obstacle>
{
    private readonly ObstacleData data;
    private readonly float gameHeight;
    
    public ObstaclePooling(ObstacleData data, float gameHeight)
    {
        this.data = data;
        this.gameHeight = gameHeight;
    }
    
    protected override Obstacle GetAvailableItem()
    {
        Obstacle obstacle;
        if (!AvailablePoolables.Any())
        {
            obstacle = new Obstacle();
            obstacle.Initialize(data, gameHeight);
            return obstacle;
        }
        obstacle = AvailablePoolables.First();
        AvailablePoolables.Remove(obstacle);
        obstacle.Initialize(data, gameHeight);
        return obstacle;
    }
}