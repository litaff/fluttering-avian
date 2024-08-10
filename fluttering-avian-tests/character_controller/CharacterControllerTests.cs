namespace fluttering_avian_tests.character_controller;

using fluttering_avian.character_controller;
using Godot;

[TestFixture]
public class CharacterControllerTests
{
    private CharacterController characterController;
    private RayCast3D ceilingRayCast;
    
    [SetUp]
    public void SetUp()
    {
        ceilingRayCast = new RayCast3D();
        
        characterController = new CharacterController();
        characterController.Initialize(ceilingRayCast, 1f, 2f);
        characterController._Ready();
    }
    
    [Test]
    public void RequestJump_SetsJumpRequestedToTrue()
    {
        characterController.RequestJump();
        
        Assert.That(characterController.JumpRequested, Is.True);
    }
    
    [Test]
    public void _PhysicsProcess_WhenJumpRequested_SetsAxisVelocity()
    {
        characterController.RequestJump();

        characterController._PhysicsProcess(0.016f);
        
        Assert.That(characterController.LinearVelocity, Is.EqualTo(new Vector3(0, characterController.JumpVelocity, 0)));
    }
    
    [Test]
    public void _PhysicsProcess_WhenJumpNotRequested_DoesNotSetAxisVelocity()
    {
        characterController._PhysicsProcess(0.016f);
        
        Assert.That(characterController.LinearVelocity, Is.EqualTo(Vector3.Zero));
    }
    
    [Test]
    public void _PhysicsProcess_WhenJumpRequestedAndShouldBoostJump_SetsAxisVelocityWithBoostedJumpVelocity()
    {
        #region Trigger ShouldBoostJump

        characterController.RequestJump();

        characterController._PhysicsProcess(0.016f);
        
        characterController.RequestJump();

        characterController._PhysicsProcess(0.016f);

        #endregion

        Assert.That(characterController.LinearVelocity.Y, Is.EqualTo(characterController.BoostedJumpVelocity));
    }
    
    [Test]
    public void _PhysicsProcess_WhenJumpRequested_ResetsJumpRequested()
    {
        characterController.RequestJump();
        
        Assert.That(characterController.JumpRequested, Is.True);
        
        characterController._PhysicsProcess(0.016f);
        
        Assert.That(characterController.JumpRequested, Is.False);
    }
    
    [TearDown]
    public void TearDown()
    {
        characterController.Dispose();
        ceilingRayCast.Dispose();
    }
}