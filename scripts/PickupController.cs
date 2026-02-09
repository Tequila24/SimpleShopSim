using System.Collections;
using System.Linq;
using Godot;


public partial class PickupController : Node
{
	[Export]
	public StackInventory Inventory = new();

	[Export]
	private Node3D _itemsVisualHolder;
	private Vector3 _itemVisualPositionStep = Vector3.One * 0.5f;
	private Vector3 _itemVisualPositionMax = Vector3.One;



	public override void _Ready()
	{
		base._Ready();

		Inventory.OnUpdated += DoUpdateVisual;
	}

	public void DoUpdateVisual()
	{
		int itemsVisualCount = _itemsVisualHolder.GetChildCount();

		if (Inventory.Count() == itemsVisualCount)
		{
			return;
		}
		else if (Inventory.Count() < itemsVisualCount)
		{
			_itemsVisualHolder.GetChild(-1).QueueFree();
		}
		else if (Inventory.Count() > itemsVisualCount)
		{
			int difference = Inventory.Count() - itemsVisualCount;

			// if no item, next will be at 0
			Vector3 nextItemVisualPos = new Vector3(0, -_itemVisualPositionStep.Y, 0);
			if (itemsVisualCount > 0) {
				nextItemVisualPos = _itemsVisualHolder.GetChild<Node3D>(-1).Position;
				
			}

			while (difference > 0)
			{
				//next position
				nextItemVisualPos.Y += _itemVisualPositionStep.Y;
				
				Node3D newItemVisual = Inventory.PeekItemAt(Inventory.Count() - difference).subScene.Instantiate<Node3D>();
				_itemsVisualHolder.AddChild(newItemVisual);
				newItemVisual.Position = nextItemVisualPos;

				difference--;
			}
		}
	}
}
