using Godot;
using Godot.NativeInterop;
using System;

public partial class PlayerController : Node
{
	[Export]
	private float _maxFreeFallVelocity = 9.0f;
	[Export]
	private float _maxWalkVelocity = 10.0f;

	CharacterBody3D _playerBody;


	Vector3 playerInputDirection = Vector3.Zero;



	public override void _Ready()
	{
		InputMaster.Instance.OnPlayerDirectionUpdated +=  DoInputDirectionUpdated;
		
		_playerBody = Utils.GetParentOfType<CharacterBody3D>(this);
	}


	public void DoInputDirectionUpdated(Vector3 newDirection)
	{
		GD.Print($"new directopn: {newDirection}");
		playerInputDirection = newDirection;
	}


	public override void _Process(double delta)
	{
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		UpdateVelocities((float)delta);

		_playerBody.MoveAndSlide();
	}


	private void UpdateVelocities(float delta)
	{
		if (_playerBody == null)
			return;

		Vector3 newVelocity = _playerBody.Velocity;

		if (!_playerBody.IsOnFloor())
		{
			newVelocity.Y = Mathf.MoveToward(newVelocity.Y, -_maxFreeFallVelocity, 9.8f * delta);
		}


		Vector3 rotatedInputDirection = CameraMaster.Instance.GetCameraYawQuat() * playerInputDirection;
		Vector3 maxDirection = rotatedInputDirection * _maxWalkVelocity;

		newVelocity.X = Mathf.Lerp(newVelocity.X, maxDirection.X, 10.00f * delta);
		newVelocity.Z = Mathf.Lerp(newVelocity.Z, maxDirection.Z, 10.00f * delta);

		_playerBody.Velocity = newVelocity;
	}
}
