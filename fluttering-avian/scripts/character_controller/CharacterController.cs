namespace fluttering_avian.character_controller;

using Godot;

public partial class CharacterController : RigidBody3D
{
	[Export]
	public RayCast3D CeilingRayCast { get; private set; }
	[Export]
	public float JumpHeight { get; private set; }
	[Export(PropertyHint.Range, "1, 2, or_greater")]
	public float RapidInputGain { get; private set; }

	private float defaultGravity;
	private float jumpVelocity;
	private bool jumpRequested;
	
	private float BoostedJumpVelocity => jumpVelocity * RapidInputGain;
	private bool ShouldBoostJump => LinearVelocity.Y > 0 && !WillHitCeiling(BoostedJumpVelocity);

	public override void _Ready()
	{
		defaultGravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
		jumpVelocity = Mathf.Sqrt(JumpHeight * 2 * defaultGravity);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!jumpRequested) return;
		
		SetAxisVelocity(new Vector3(LinearVelocity.X, ShouldBoostJump ? BoostedJumpVelocity : jumpVelocity, LinearVelocity.Z));
		
		jumpRequested = false;
		
		base._PhysicsProcess(delta);
	}

	// TODO: Implement input handling outside of CharacterController.
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		
		if(@event is InputEventKey { Keycode: Key.Space, Pressed: true, Echo: false })
		{
			jumpRequested = true;
		}
	}
	
	/// <summary>
	/// Determines if the character will hit a ceiling with the given vertical velocity.
	/// If velocity is negative, this method will always return false.
	/// </summary>
	private bool WillHitCeiling(float verticalVelocity)
	{
		if (verticalVelocity <= 0) return false;
		
		// TODO: This is good enough, but not perfectly accurate.
		var maxDistanceWithVelocity = Mathf.Pow(verticalVelocity, 2) / (2 * defaultGravity);
		CeilingRayCast.TargetPosition = new Vector3(0, maxDistanceWithVelocity, 0);
		return CeilingRayCast.IsColliding();
	}
}