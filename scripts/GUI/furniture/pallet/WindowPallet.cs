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

		Node playerNode = GetTree().GetNodesInGroup("PlayerGroup")[0];
		var pickupControl = Utils.GetFirstChildOfType<PickupController>(playerNode);

		if (pickupControl.Inventory.IsFull)
			return;

		if (_pallet.Data.Contents.TryPopItem() is ItemData topPalletItem)
		{
			pickupControl.Inventory.PushItem(topPalletItem);
		}
	}

	private void DoTryPut()
	{
		if (_pallet == null)
			return;

		if (_pallet.Data.Contents.IsFull)
			return;

		Node playerNode = GetTree().GetNodesInGroup("PlayerGroup")[0];
		var pickupControl = Utils.GetFirstChildOfType<PickupController>(playerNode);

		if (pickupControl.Inventory.PeekTopItem() is ItemData topItem)
		{
			bool result = _pallet.Data.Contents.PushItem(topItem);
			if (result)
			{
				pickupControl.Inventory.TryPopItem();
			}
		}
	}
}
