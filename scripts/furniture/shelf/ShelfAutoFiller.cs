using Godot;
using System;

public partial class ShelfAutoFiller : Node
{
	[Export]
	private Interactive _interactZone;
	[Export]
	private Timer _interactionTimer;
	[Export]
	private ShelfLogic _shelfLogic;



	public override void _Ready()
	{
		_interactZone.OnEntered += (Node3D unused) => { _interactionTimer.Start(); };
		_interactZone.OnExited += (Node3D unused) => { _interactionTimer.Stop(); };

		_interactionTimer.Timeout += DoFill;
	}

	private void DoFill()
	{
		var player = Global.GetPlayerNode();
		PickupController pickup = Utils.GetFirstChildOfType<PickupController>(player);
		if (pickup == null)
			return;

		var playerTopItem = pickup.Inventory.PeekTopItem();
		if (playerTopItem != _shelfLogic.ShelfItem)
		{
			_interactionTimer.Stop();
			return;
		}

		if (_shelfLogic.TryAddItem(playerTopItem))
		{
			pickup.Inventory.TryPopItem();
		}
	}
}
