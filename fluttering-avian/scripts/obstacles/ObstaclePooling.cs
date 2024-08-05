namespace fluttering_avian.obstacles;

using System.Linq;
using Godot;
using Pooling;

public class ObstaclePooling : Pooling<Obstacle>
{
    private readonly PackedScene template;
    
    public ObstaclePooling(PackedScene template)
    {
        this.template = template;
    }
    
    protected override Obstacle GetAvailableItem()
    {
        if (!AvailablePoolables.Any()) return template.Instantiate<Obstacle>();
        var obstacle = AvailablePoolables.First();
        AvailablePoolables.Remove(obstacle);
        return obstacle;
    }
}