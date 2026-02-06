using Godot;
using System;

public partial class NPCNavigation : NavigationAgent3D
{
	[Export]
	private CharacterBody3D _npcBody;

	[Export]
	public float _maxSpeed = 6;



	public override void _Ready()
	{
		base._Ready();

		NavigationMaster.Instance.BakeFinished += DoNavmeshBakeFinished;
		VelocityComputed += DoVelocityComputed;
	}

	private void DoNavmeshBakeFinished()
	{
		// TargetPosition = TargetPosition;
	}

	public void NavigateTo(Vector3 newWorldPosition)
	{
		TargetPosition = newWorldPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var nextPointGlobal = GetNextPathPosition();
		var localSpeedToNext = (nextPointGlobal - _npcBody.GlobalPosition).Normalized();
		// localSpeedToNext.Y = 0;
		Velocity = localSpeedToNext;
	}


	public void DoVelocityComputed(Vector3 newVelocity)
	{
		_npcBody.Velocity = newVelocity * _maxSpeed;
		_npcBody.MoveAndSlide();
	}
}
