namespace fluttering_avian.character_controller;

using System.Linq;
using Godot;
using Godot.Collections;

public partial class CharacterController : RigidBody3D
{
	[Export]
	public Array<RayCast3D> CeilingRayCast { get; private set; }
	[Export]
	public float JumpHeight { get; private set; }
	[Export(PropertyHint.Range, "1, 2, or_greater")]
	public float RapidInputGain { get; private set; }

	private float defaultGravity;

	public float JumpVelocity { get; private set; }
	public float BoostedJumpVelocity => JumpVelocity * RapidInputGain;
	public bool JumpRequested { get; private set; }

	private bool ShouldBoostJump => LinearVelocity.Y > 0 && !WillHitCeiling(BoostedJumpVelocity);

	public override void _Ready()
	{
		defaultGravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
		JumpVelocity = Mathf.Sqrt(JumpHeight * 2 * defaultGravity);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!JumpRequested) return;
		
		SetAxisVelocity(new Vector3(LinearVelocity.X, ShouldBoostJump ? BoostedJumpVelocity : JumpVelocity, LinearVelocity.Z));
		
		JumpRequested = false;
		
		base._PhysicsProcess(delta);
	}

	/// <summary>
	/// Implemented for testing purposes, can be used as an initializer if setting properties via the inspector is not possible.
	/// </summary>
	/// <param name="ceilingRayCast">Raycast which determines whether the character will hit the ceiling.</param>
	/// <param name="jumpHeight">The height of the jump.</param>
	/// <param name="rapidInputGain"><see cref="JumpVelocity"/> multiplier if rapid input is detected.</param>
	public void Initialize(RayCast3D[] ceilingRayCast, float jumpHeight, float rapidInputGain)
	{
		CeilingRayCast = new Array<RayCast3D>(ceilingRayCast);
		JumpHeight = jumpHeight;
		RapidInputGain = rapidInputGain;
	}
	
	/// <summary>
	/// When called, the character will jump on the next physics process.
	/// </summary>
	public void RequestJump()
	{
		JumpRequested = true;
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

		foreach (var rayCast3D in CeilingRayCast)
		{
			rayCast3D.TargetPosition = new Vector3(0, maxDistanceWithVelocity, 0);
		}
		
		return CeilingRayCast.Any(rayCast => rayCast.IsColliding());
	}
}