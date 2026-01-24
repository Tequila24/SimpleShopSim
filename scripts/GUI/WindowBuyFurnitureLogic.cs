using Godot;
using System;

public partial class WindowBuyFurnitureLogic : MarginContainer
{
	[Export]
	private VBoxContainer _vBox;
	[Export]
	private HBoxContainer _hBox;

	[Export]
	private PackedScene _furnitureButton;


	public override void _Ready()
	{
		Utils.ClearChildren(_hBox);

		foreach (var fData in AccountWrapper.GetUnlockedFurniture())
		{
			AddNewButton(fData);
		}
	}

	private void AddNewButton(FurnitureData newFurnitureData)
	{
		var newButton =_furnitureButton.Instantiate<Control>();

		if (newButton.FindChild("LabelName") is Label label)
		{
			label.Text = newFurnitureData.name;
		}

		if (newButton.FindChild("LabelName") is TextureRect textRect)
		{
			textRect.Texture = newFurnitureData.image2D;
		}

		_hBox.AddChild(newButton);

		if (newButton.FindChild("Button") is Button btn)
		{
			btn.ButtonUp += () => { DoFurnitureSelected(newFurnitureData); };
		}
	}

	private void DoFurnitureSelected(FurnitureData data)
	{
		GD.Print($"Clicked on {data.name}");
		Global.TryPlaceObject(data);
		this.QueueFree();
	}
}
