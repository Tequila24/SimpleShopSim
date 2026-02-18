using Godot;
using System;

public partial class InteractionZoneRotator : Node3D
{
	private float _targetRotation = 0;

	private Tween rotationTween;

	public override void _Ready()
	{
		base._Ready();

		_targetRotation = this.RotationDegrees.Y;

		CameraMaster.Instance.OnCameraRotated += DoCameraRotationChanged;

		DoCameraRotationChanged();
	}

	private void DoCameraRotationChanged()
	{
		var cameraYawRounded = Mathf.Round(CameraMaster.Instance.CameraYawAngle / 90) * 90;

		if (_targetRotation != cameraYawRounded)
		{
			UpdateRotation(cameraYawRounded);
		}
		// GD.Print($"rounded rotation {_roundedCameraAngle}");
		// this.RotationDegrees = new(0, _roundedCameraAngle, 0);
	}

	public void UpdateRotation(float newRotationY)
	{
		if (rotationTween != null)
		{
			rotationTween.Kill();
			rotationTween = null;
		}

		rotationTween = CreateTween();
		rotationTween.TweenProperty(this, "quaternion", Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(newRotationY), 0)), 0.1f);

		_targetRotation = newRotationY;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}
}