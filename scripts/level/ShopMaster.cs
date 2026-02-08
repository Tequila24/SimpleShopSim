using Godot;


public partial class ShopMaster : Node
{
	public static ShopMaster Instance
	{ private set; get; }

	public bool IsShopOpen = false;



	public override void _EnterTree()
	{
		base._EnterTree();

		Instance ??= this;
	}

	public override void _Ready()
	{
		base._Ready();

		SignalBus.OnSwitchShopState += FlipShopState;
	}


	public void FlipShopState()
	{
		IsShopOpen = !IsShopOpen;
		SignalBus.OnShopStateUpdated?.Invoke(IsShopOpen);
	}
}
