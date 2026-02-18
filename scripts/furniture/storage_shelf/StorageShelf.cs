using Godot;


public partial class StorageShelf : StaticBody3D/*, IAutoExchange*/
{
	[Export]
	private SingleItemInventory _contents;

	[Export]
	private Node3D _itemsVisualHolder;
	[Export]
	private Vector3I _itemVisualMaxCount = Vector3I.One;



	public override void _Ready()
	{
		base._Ready();

		if (_contents == null)
			_contents = new SingleItemInventory();

		_itemsVisualHolder ??= this.FindChild("ItemsVisualHolder") as Node3D;

		// _contents.OnUpdated += UpdateVisual;
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		// int itemsVisualCount = _itemsVisualHolder.GetChildCount();

		// if (_contents.Count == itemsVisualCount)
		// {
		// 	return;
		// }
		// else if (_contents.Count < itemsVisualCount)
		// {
		// 	int difference = itemsVisualCount - _contents.Count;

		// 	while (difference > 0)
		// 	{
		// 		_itemsVisualHolder.GetChild(-difference).QueueFree();
		// 		difference--;
		// 	}
		// }
		// else if (_contents.Count > itemsVisualCount)
		// {
		// 	int difference = _contents.Count - itemsVisualCount;

		// 	// if no item, next will be at 0
		// 	Vector3 nextItemVisualPos = new Vector3(-_itemVisualPositionStep.X, 0, 0);
		// 	if (itemsVisualCount > 0) {
		// 		nextItemVisualPos = _itemsVisualHolder.GetChild<Node3D>(-1).Position;

		// 	}

		// 	while (difference > 0)
		// 	{
		// 		//next position
		// 		nextItemVisualPos.X += _itemVisualPositionStep.X;
		// 		if (nextItemVisualPos.X > _itemVisualPositionMax.X)
		// 		{
		// 			nextItemVisualPos.X = 0;
		// 			nextItemVisualPos.Z += _itemVisualPositionStep.Z;
		// 			if (nextItemVisualPos.Z > _itemVisualPositionMax.Z) {
		// 				nextItemVisualPos.Z = 0;
		// 				nextItemVisualPos.Y += _itemVisualPositionStep.Y;
		// 			}
		// 		}


		// 		Node3D newItemVisual = _contents.Contents.PeekItemAt(_contents.Contents.Count - difference).subScene.Instantiate<Node3D>();
		// 		_itemsVisualHolder.AddChild(newItemVisual);
		// 		newItemVisual.Position = nextItemVisualPos;

		// 		difference--;
		// 	}
		// }
	}
}
