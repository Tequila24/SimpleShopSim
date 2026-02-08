using Godot;
 

public partial class HUDLogic : Control
{
	public static HUDLogic Root
	{
		private set; get;
	}

	[Export]
	private ItemCounterLogic moneyCounter;
	[Export]
	private Button buttonOpenFurnitureShop;
	[Export]
	private PackedScene windowFurnitureShop;



	public override void _EnterTree()
	{
		base._EnterTree();

		Root ??= this;
	}

	public override void _Ready()
	{
		base._Ready();

		InitMoneyCounter();
		SignalBus.OnAccMoneyChanged += UpdateMoneyCounter;
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
}
