using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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

	private Godot.Collections.Array<ItemData> _availableItemsToTake;


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
		if (_from == null || _to == null)
		{
			GD.Print($"Error starting exchange:\n\t From Exchanger: {(_to as Node3D).Name} \n\t To Exchanger: {(_to as Node3D).Name}");
			return;
		}

		_availableItemsToTake = _from.GetAutoAvailableItems();
		if (_availableItemsToTake.Count == 0 || _availableItemsToTake[0] == null)
			return;

		_exchangeTimer.Start();
	}

	private void StopExchange()
	{
		_exchangeTimer.Stop();
	}

	private void DoTryExchange()
	{
		if (_availableItemsToTake.Count == 0) {
			StopExchange();
			return;
		}

		GD.Print($"next Index to take {_availableItemsToTake.Count - 1}");

		var nextItemToTake = _availableItemsToTake.ElementAt(_availableItemsToTake.Count - 1);

		if (nextItemToTake == null)
			return;

		if (!_from.HasAutoItem(new(nextItemToTake, 1)))
		{
			_availableItemsToTake.RemoveAt(_availableItemsToTake.Count - 1);
			return;
		}


		if (!_to.TryAutoPutItem(new(nextItemToTake, 1)))
		{
			_availableItemsToTake.RemoveAt(_availableItemsToTake.Count - 1);
			return;
		} else
		{
			GD.Print($"Putting item {1}");
		}

		if (!_from.TryAutoTakeItem(new(nextItemToTake, 1)))
		{
			// _to.TryAutoTakeItem(new(nextItemToTake, 1));
			_availableItemsToTake.RemoveAt(_availableItemsToTake.Count - 1);
			return;
		}
	}
}
