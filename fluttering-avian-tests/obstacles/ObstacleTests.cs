namespace fluttering_avian_tests.obstacles;

using fluttering_avian.obstacles;
using Godot;
using Godot.Collections;
using mock;

[TestFixture]
public class ObstacleTests
{
    private Obstacle obstacle;
    private Area3D solidZone;
    private Area3D gapZone;
    private MockRandom mockRandom;
    
    [SetUp]
    public void SetUp()
    {
        mockRandom = new MockRandom();
        
        solidZone = new Area3D();
        gapZone = new Area3D();
        
        obstacle = new Obstacle();
        obstacle.Initialize(10, 0, 
            new Array<Area3D>{solidZone}, new Array<Area3D>{gapZone});
    }
    
    [Test]
    public void GetRandomSpawnHeight_ReturnsRandomValueBetweenMinAndMax()
    {
        mockRandom.ReturnsUpperBound = false;
        var randomSpawnHeight = obstacle.GetRandomSpawnHeight(mockRandom);

        Assert.That(randomSpawnHeight, Is.InRange(0, 10));
        
        mockRandom.ReturnsUpperBound = true;
        randomSpawnHeight = obstacle.GetRandomSpawnHeight(mockRandom);
        
        Assert.That(randomSpawnHeight, Is.InRange(0, 10));
    }
    
    [Test]
    public void Free_InvokesOnFreedEvent()
    {
        var wasOnFreedInvoked = false;
        obstacle.OnFreed += _ => wasOnFreedInvoked = true;
        
        obstacle.Free();
        
        Assert.That(wasOnFreedInvoked, Is.True);
    }
    
    // Too much of a bother to test RegisterHandlers and UnregisterHandlers.
    
    [TearDown]
    public void TearDown()
    {
        obstacle.Dispose();
        solidZone.Dispose();
        gapZone.Dispose();
    }
}