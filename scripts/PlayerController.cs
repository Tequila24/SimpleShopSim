using Godot;


public partial class PlayerController : Node
{
	[Export]
	private float _maxFreeFallVelocity = 9.0f;
	[Export]
	private float _maxWalkVelocity = 10.0f;

	public enum CharLocoState
	{
		NA,
		IDLE,
		FALLING,
		WALKING
	}

	CharacterBody3D _playerBody;


	[Export]
	private AnimationPlayer _animPlayer;

	[Export]
	private CharLocoState _currentLocomotion = CharLocoState.NA;
	Vector3 playerInputDirection = Vector3.Zero;



	public override void _Ready()
	{
		InputMaster.Instance.OnPlayerDirectionUpdated += DoInputDirectionUpdated;

		_playerBody = Utils.GetParentOfType<CharacterBody3D>(this);
	}


	public void DoInputDirectionUpdated(Vector3 newDirection)
	{
		playerInputDirection = newDirection;
	}


	public override void _Process(double delta)
	{
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		UpdateVelocities((float)delta);
		UpdateRotation();
		UpdateCharLocoState();

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


	private void UpdateRotation()
	{
		Vector3 newRotation = _playerBody.Rotation;

		var velocityFlat = _playerBody.Velocity * new Vector3(1, 0, 1);
		newRotation.Y = Mathf.Pi - velocityFlat.SignedAngleTo(Vector3.Forward, Vector3.Up);

		_playerBody.Rotation = newRotation;
	}


	private void UpdateCharLocoState()
	{
		var prevState = _currentLocomotion;

		if (!_playerBody.IsOnFloor())
		{
			_currentLocomotion = CharLocoState.FALLING;
		}
		else
		{
			if (_playerBody.Velocity.LengthSquared() <= 0.5f)
			{
				_currentLocomotion = CharLocoState.IDLE;
			}
			else
			{
				_currentLocomotion = CharLocoState.WALKING;
			}
		}

		if (prevState != _currentLocomotion)
			UpdateCharAnim();
	}


	private void UpdateCharAnim()
	{
		switch (_currentLocomotion)
		{
			case CharLocoState.NA:
				_animPlayer.Stop();
				break;

			case CharLocoState.IDLE:
				_animPlayer.Play("Anim_char/Root_Idle");
				break;

			case CharLocoState.FALLING:
				_animPlayer.Stop();
				break;

			case CharLocoState.WALKING:
				_animPlayer.Play("Anim_char/Root_Run");
				break;

			default:
				_animPlayer.Stop();
				break;
		}
	}
}
