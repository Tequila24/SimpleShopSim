using System.Runtime.Serialization.Formatters;
using Godot;


public partial class WindowFlipSignLogic : Control
{
	[Export]
	Button _button;
	[Export]
	Control _viewOpen;
	[Export]
	Control _viewCLosed;



	public override void _Ready()
	{
		_button.ButtonUp += DoSwitchButtonPressed;

		DoUpdateSignVisual(ShopMaster.Instance.IsShopOpen);

		SignalBus.OnShopStateUpdated += (bool newState) => { DoUpdateSignVisual(newState); };
	}

	private void DoSwitchButtonPressed()
	{
		// GD.Print($"I AM TEST {this.Name}");
		SignalBus.OnSwitchShopState();
	}

	public void DoUpdateSignVisual(bool isOpen, bool animated = false)
	{
		if (!animated)
		{
			_viewOpen.Visible = isOpen;
			_viewCLosed.Visible = !isOpen;
			return;
		}

		if (isOpen)
		{
			
		}
		else
		{

		}
	}

}
