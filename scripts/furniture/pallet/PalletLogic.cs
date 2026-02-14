using Godot;
using System;
using System.Threading;

public partial class PalletLogic : Node3D, IAutoExchange
{
	[Export]
	private PalletData _data;
	public PalletData Data => _data;

	[Export]
	private Node3D _itemsVisualHolder;
	private Vector3 _itemVisualPositionStep = Vector3.One * 0.5f;
	private Vector3 _itemVisualPositionMax = Vector3.One;


	public void InitWithData(PalletData newData)
	{
		_data = newData;
		_itemsVisualHolder ??= this.FindChild("ItemsVisualHolder") as Node3D;
	}

	public override void _Ready()
	{
		base._Ready();
		
		_data.Contents.OnUpdated += UpdateVisual;
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		if (_data.Contents == null)
			return;

		int itemsVisualCount = _itemsVisualHolder.GetChildCount();

		if (_data.Contents.Count == itemsVisualCount)
		{
			return;
		}
		else if (_data.Contents.Count < itemsVisualCount)
		{
			_itemsVisualHolder.GetChild(-1).QueueFree();
		}
		else if (_data.Contents.Count > itemsVisualCount)
		{
			int difference = _data.Contents.Count - itemsVisualCount;

			// if no item, next will be at 0
			Vector3 nextItemVisualPos = new Vector3(-_itemVisualPositionStep.X, 0, 0);
			if (itemsVisualCount > 0) {
				nextItemVisualPos = _itemsVisualHolder.GetChild<Node3D>(-1).Position;
				
			}

			while (difference > 0)
			{
				//next position
				nextItemVisualPos.X += _itemVisualPositionStep.X;
				if (nextItemVisualPos.X > _itemVisualPositionMax.X)
				{
					nextItemVisualPos.X = 0;
					nextItemVisualPos.Z += _itemVisualPositionStep.Z;
					if (nextItemVisualPos.Z > _itemVisualPositionMax.Z) {
						nextItemVisualPos.Z = 0;
						nextItemVisualPos.Y += _itemVisualPositionStep.Y;
					}
				}

				
				Node3D newItemVisual = _data.Contents.PeekItemAt(_data.Contents.Count - difference).subScene.Instantiate<Node3D>();
				_itemsVisualHolder.AddChild(newItemVisual);
				newItemVisual.Position = nextItemVisualPos;

				difference--;
			}
		}
	}
	
	public ItemData GetAutoNextItem()
	{
		return Data.Contents.GetAutoNextItem();
	}

	public ItemData TryAutoTakeItem()
	{
		return Data.Contents.TryPopItem();
	}

	public bool TryAutoPutItem(ItemData newItem)
	{
		return Data.Contents.TryPushItem(newItem);
	}
}
