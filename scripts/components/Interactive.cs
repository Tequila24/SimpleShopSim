using Godot;
using System;

public partial class Interactive : Node
{
	[Export]
	private Godot.Collections.Array<string> _groupNames = [];
	[Export]
	private bool _checkAreas;
	[Export]
	private bool _checkBodies;

	[Export]
	private Area3D _interactionZone;

	public Action<Node3D> OnEntered;
	public Action<Node3D> OnExited;



	public override void _Ready()
	{
		if (_checkAreas)
		{
			_interactionZone.AreaEntered += AreaEntered;
			_interactionZone.AreaExited += AreaExited;
		}

		if (_checkBodies)
		{
			_interactionZone.BodyEntered += DoBodyEntered;
			_interactionZone.BodyExited += DoBodyExited;
		}
	}

	private void AreaEntered(Area3D otherArea)
	{
		foreach (var groupName in _groupNames)
		{
			if (otherArea.GetGroups().Contains(groupName))
			{
				OnEntered(otherArea);
				return;
			}
		}
	}

	private void AreaExited(Area3D otherArea)
	{
		foreach (var groupName in _groupNames)
		{
			if (otherArea.GetGroups().Contains(groupName))
			{
				OnExited(otherArea);
				return;
			}
		}
	}

	private void DoBodyEntered(Node3D body)
	{
		foreach (var groupName in _groupNames)
		{
			if (body.GetGroups().Contains(groupName))
			{
				OnEntered(body);
				return;
			}
		}
	}

	private void DoBodyExited(Node3D body)
	{
		foreach (var groupName in _groupNames)
		{
			if (body.GetGroups().Contains(groupName))
			{
				OnExited(body);
				return;
			}
		}
	}
}
