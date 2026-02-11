using System.Collections;
using System.Linq;
using Godot;


public partial class PickupController : Node
{
	[Export]
	private StackInventory _inventory;
	public StackInventory Inventory => _inventory;

	[Export]
	private Node3D _itemsVisualHolder;
	private Vector3 _itemVisualPositionStep = Vector3.One * 0.5f;
	private Vector3 _itemVisualPositionMax = Vector3.One;



	public override void _Ready()
	{
		base._Ready();

		if (_inventory == null)
			_inventory = new StackInventory();

		_inventory.OnUpdated += DoUpdateVisual;
		DoUpdateVisual();
	}

	public void DoUpdateVisual()
	{
		int itemsVisualCount = _itemsVisualHolder.GetChildCount();

		if (_inventory.Count == itemsVisualCount)
		{
			return;
		}
		else if (_inventory.Count < itemsVisualCount)
		{
			_itemsVisualHolder.GetChild(-1).QueueFree();
		}
		else if (_inventory.Count > itemsVisualCount)
		{
			int difference = _inventory.Count - itemsVisualCount;

			// if no item, next will be at 0
			Vector3 nextItemVisualPos = new Vector3(0, -_itemVisualPositionStep.Y, 0);
			if (itemsVisualCount > 0) {
				nextItemVisualPos = _itemsVisualHolder.GetChild<Node3D>(-1).Position;
				
			}

			while (difference > 0)
			{
				//next position
				nextItemVisualPos.Y += _itemVisualPositionStep.Y;
				
				Node3D newItemVisual = _inventory.PeekItemAt(_inventory.Count - difference).subScene.Instantiate<Node3D>();
				_itemsVisualHolder.AddChild(newItemVisual);
				newItemVisual.Position = nextItemVisualPos;

				difference--;
			}
		}
	}
}
