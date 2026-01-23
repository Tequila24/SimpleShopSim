using Godot;


public partial class HUDLogic : Control
{
	[Export]
	private ItemCounterLogic moneyCounter;
	[Export]
	private Button buttonOpenFurnitureShop;
	[Export]
	private PackedScene windowFurnitureShop;


	public override void _Ready()
	{
		base._Ready();

		buttonOpenFurnitureShop.ButtonUp += DoOpenFurnitureShop;

		UpdateMoneyCounter();
	}

	public void UpdateMoneyCounter()
	{
		moneyCounter.ItemImage.Texture = AccountWrapper.GetAccMoney().item.image;
		moneyCounter.ItemCountLabel.Text = AccountWrapper.GetAccMoney().count.ToString();
	}

	public void DoOpenFurnitureShop()
	{
		var newWindow = windowFurnitureShop.Instantiate<Control>();
		this.AddChild(newWindow);
	}
}
