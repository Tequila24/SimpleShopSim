using Godot;


public partial class AInventory : Resource
{
	[Export]
	protected Godot.Collections.Array<ItemCountData> _itemsStack = [];

	[Export]
	protected int _maxCapacity = -1;
	public int MaxCapacity => _maxCapacity;

	public int Count => _itemsStack.Count;
	
	public bool IsFull => _maxCapacity > 0 && _itemsStack.Count >= _maxCapacity;
	public bool IsEmpty => (Count == 0);

	[Signal]
	public delegate void OnUpdatedEventHandler();
	public void EmitOnUpdated()
	{ EmitSignal(SignalName.OnUpdated); }
}
