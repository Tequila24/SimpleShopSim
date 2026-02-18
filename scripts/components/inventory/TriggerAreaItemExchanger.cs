using System.Linq;
using Godot;

public partial class TriggerAreaItemExchanger : Node
{
	enum ExchangeDirection
	{
		FROM_ATTACHED,
		TO_ATTACHED
	}
	[Export]
	private ExchangeDirection _exchangeDirection = ExchangeDirection.FROM_ATTACHED;
	
	[Export]
	private Interactive _interactZone;
	[Export]
	private Timer _exchangeTimer;

	// private IAutoExchange _attachedExchanger;
	// private IAutoExchange _otherExchanger;

	private IAutoExchange _from;
	private IAutoExchange _to;

	private Godot.Collections.Array<ItemData> _availableItemsFilter;


	public override void _Ready()
	{
		ref IAutoExchange attachedExchanger = ref (_exchangeDirection == ExchangeDirection.FROM_ATTACHED ? ref _from : ref _to);
		attachedExchanger = Utils.GetFirstChildOfType<IAutoExchange>(GetParent());

		if (_exchangeTimer == null)
		{
			_exchangeTimer = new Timer();
			_exchangeTimer.OneShot = false;
			this.AddChild(_exchangeTimer);
			_exchangeTimer.WaitTime = 0.3f;
		}

		_interactZone.OnEntered += (Node3D enteredNode) =>
		{
			ref IAutoExchange _otherExchanger = ref (_exchangeDirection == ExchangeDirection.FROM_ATTACHED ? ref _to : ref _from);
			_otherExchanger = Utils.GetFirstChildOfType<IAutoExchange>(enteredNode);

			InitExchange();
		};

		_interactZone.OnExited += (Node3D unused) =>
		{
			ref IAutoExchange _otherExchanger = ref (_exchangeDirection == ExchangeDirection.FROM_ATTACHED ? ref _to : ref _from);
			_otherExchanger = null;

			StopExchange();
		};

		_exchangeTimer.Timeout += DoTryExchange;
	}

	private void InitExchange()
	{
		if (_from == null || _to == null) {
			GD.Print($"Error starting exchange:\n\t From Exchanger: {(_to as Node3D).Name} \n\t To Exchanger: {(_to as Node3D).Name}");
			return;
		}

		_availableItemsFilter = _to.GetAutoAvailableItems();
		if (_availableItemsFilter.Count == 0 || _availableItemsFilter[0] == null)
			return;

		_exchangeTimer.Start();
	}

	private void StopExchange()
	{
		_exchangeTimer.Stop();
	}

	private void DoTryExchange()
	{
		if (_availableItemsFilter.Count == 0)
			StopExchange();

		var nextItemType = _availableItemsFilter.First();
		GD.Print($"next item tpe {nextItemType.name}");

		if (!_from.HasAutoItem(new(nextItemType, 1)))
		{
			_availableItemsFilter.Remove(nextItemType);
			return;
		}

		if (!_to.TryAutoPutItem(new(nextItemType, 1))) {
			_availableItemsFilter.Remove(nextItemType);
			return;
		}

		_from.TryAutoTakeItem(new(nextItemType, 1));

		return;
	}
}
