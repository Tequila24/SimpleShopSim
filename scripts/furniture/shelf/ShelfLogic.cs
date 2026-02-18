using Godot;


public partial class ShelfLogic : Node3D
{
	[Export]
	private SingleItemInventory _inventory;
	[Export]
	private Node3D _visualsRoot;
	
	private Godot.Collections.Array<Node3D> _visualHolders = new();



	public override void _Ready()
	{
		base._Ready();

		foreach (var holder in _visualsRoot.GetChildren())
		{
			_visualHolders.Add(holder as Node3D);
		}

		UpdateVisual();
	}

	private void UpdateVisual()
	{
		// int itemsVisualCount = 0;
		// foreach (var vHolder in _visualHolders)
		// {
		// 	if (vHolder.GetChildCount() > 0)
		// 		itemsVisualCount++;
		// }


		// if (_data.contents.count == itemsVisualCount)
		// {
		// 	return;
		// }
		// else if (_data.contents.count < itemsVisualCount)
		// {
		// 	int difference = itemsVisualCount - _data.contents.count;
		// 	// GD.Print($"data > visual, difference: {difference}");

		// 	while (difference > 0)
		// 	{
		// 		Utils.ClearChildren(_visualHolders[itemsVisualCount - difference]);
		// 		difference--;
		// 	}
		// }
		// else if (_data.contents.count > itemsVisualCount)
		// {
		// 	int difference = _data.contents.count - itemsVisualCount;

		// 	// GD.Print($"visual > data, difference: {difference} {itemsVisualCount}");

		// 	int i = 0;
		// 	while (i < difference)
		// 	{
		// 		Node3D newItemVisual = ShelfItem.subScene.Instantiate<Node3D>();
		// 		_visualHolders[itemsVisualCount + i].AddChild(newItemVisual);
		// 		i++;
		// 	}
		// }
	}
}
