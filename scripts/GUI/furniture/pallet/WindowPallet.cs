using Godot;


public partial class WindowPallet : Control
{
	[Export]
	private Button _buttonTake;
	[Export]
	private Button _buttonPut;

	PalletLogic _pallet;


	
	public override void _Ready()
	{
		_pallet = Utils.GetParentOfType<PalletLogic>(this);

		_buttonTake.ButtonUp += DoTryTake;
		_buttonPut.ButtonUp += DoTryPut;
	}

	private void DoTryTake()
	{
		if (_pallet == null)
			return;

		if (_pallet.TryTakeItem() is ItemData item)
		{
			Node playerNode = GetTree().GetNodesInGroup("PlayerGroup")[0];
			var pickupControl = Utils.GetFirstChildOfType<PickupController>(playerNode);
			pickupControl.Inventory.PushItem(item);
		}
	}

	private void DoTryPut()
	{
		if (_pallet == null)
			return;

		Node playerNode = GetTree().GetNodesInGroup("PlayerGroup")[0];
		var pickupControl = Utils.GetFirstChildOfType<PickupController>(playerNode);
		var playerTopItem = pickupControl.Inventory.PeekTopItem();
		
		if (_pallet.TryAddItem(playerTopItem))
		{
			pickupControl.Inventory.PopItem();
		}
	}
}
