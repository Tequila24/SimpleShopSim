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

		InitMoneyCounter();
		AccountWrapper.Instance.OnAccountMoneyChanged += UpdateMoneyCounter;
	}

	public void InitMoneyCounter()
	{
		moneyCounter.ItemImage.Texture = AccountWrapper.GetAccMoney().item.image;
		moneyCounter.ItemCountLabel.Text = AccountWrapper.GetAccMoney().count.ToString();
	}
	public void UpdateMoneyCounter(int newCount, int oldCount)
	{
		moneyCounter.ItemCountLabel.Text = newCount.ToString();
	}

	public void DoOpenFurnitureShop()
	{
		var newWindow = windowFurnitureShop.Instantiate<Control>();
		this.AddChild(newWindow);
	}
}
