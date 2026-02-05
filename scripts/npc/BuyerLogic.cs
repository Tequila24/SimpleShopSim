using Godot;
using System.Linq;


public partial class BuyerLogic : Node
{
	enum State
	{
		DISABLED,
		IDLE,
		SEARCHING_ITEMS,
		SEARCHING_CASHIER,
		LEAVING
	}

	[Export]
	private CharacterBody3D _npcBody;
	[Export]
	private NPCNavigation _npcNav;

	private State _currentState = State.DISABLED;

	[Export]
	public Godot.Collections.Array<ItemData> _wantedItems = [];
	private Godot.Collections.Array<ItemData> _foundItems = [];
	private ShelfLogic _cachedShelf = null;



	public override void _Ready()
	{
		_npcNav.NavigationFinished += DoTargetReached;

		var delayStart = CreateTween();
		delayStart.TweenInterval(1.0f);
		delayStart.TweenCallback(Callable.From(
			() => { _currentState = State.IDLE; }
		));
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (_currentState == State.IDLE)
			UpdateState();
	}


	private void UpdateState()
	{
		if (_wantedItems.Count > 0)
		{
			_currentState = State.SEARCHING_ITEMS;
			// GD.Print($"NPC {_npcBody.Name} searching items");
			TryFindNextItem();
		}
		else
		{
			if (_foundItems.Count > 0)
			{
				_currentState = State.SEARCHING_CASHIER;
				// GD.Print($"NPC {_npcBody.Name} searching cash register");
				TryFindCashier();
			}
			else
			{
				_currentState = State.LEAVING;
				// GD.Print($"NPC {_npcBody.Name} leaving shop");
				LeaveShop();
			}
		}
	}

	private void TryFindNextItem()
	{
		var nextItem = _wantedItems.Last();
		// GD.Print($"Next searched item: {nextItem.name}");

		var newShelf = LevelMaster.Instance.TryFindShelfWithItem(nextItem);
		if (newShelf == null)
		{
			// GD.Print($"{_npcBody.Name} no shelf with {nextItem.name} found");
			_wantedItems.RemoveAt(_wantedItems.Count - 1);
			_currentState = State.IDLE;
		}

		if (_cachedShelf == newShelf)
		{
			DoTargetReached();
			return;
		}
		else
		{
			_cachedShelf = newShelf;
			_npcNav.NavigateTo(_cachedShelf.GetInteractionPoint());
		}
	}

	private void TryFindCashier()
	{
		var cashier = LevelMaster.Instance.TryFindCashRegister();
		if (cashier == null)
		{
			// GD.Print($"{_npcBody.Name} no cash register found, leaving");
			_currentState = State.LEAVING;
			LeaveShop();
			return;
		}

		_npcNav.NavigateTo(cashier.GlobalPosition);
	}

	private void LeaveShop()
	{
		_npcNav.NavigateTo(NPCSpawner.Instance._NPCDespawnPoint.GlobalPosition);
	}

	private void DoTargetReached()
	{
		if (_currentState == State.SEARCHING_ITEMS)
		{
			// GD.Print($"NPC {_npcBody.Name} taking items");
			TryTakeItemFronShelf();
		}

		if (_currentState == State.SEARCHING_CASHIER)
		{
			// GD.Print($"NPC {_npcBody.Name} paying");
			int summ = _foundItems.Aggregate(0, (total, next) => total + next.price);
			AccountWrapper.ChangeAccMoney(summ);
			_foundItems.Clear();
			_currentState = State.IDLE;
		}

		if (_currentState == State.LEAVING)
		{
			// GD.Print($"NPC {_npcBody.Name} leaving this world");
			_npcBody.QueueFree();
		}
	}

	private void TryTakeItemFronShelf()
	{
		float distanceToShelf = (_npcBody.Position - _cachedShelf.GetInteractionPoint()).Length();
		GD.Print($"Distance To Shelf {distanceToShelf}");
		if (distanceToShelf > 1.0f)
		{
			_cachedShelf = null;
			_currentState = State.IDLE;
			return;
		}

		bool result = _cachedShelf.TryTakeItem(_wantedItems.Last());
		// GD.Print($"NPC {_npcBody.Name} took {_wantedItems.Last().name} from {_cachedShelf.Name}");
		if (result)
		{
			_foundItems.Add(_wantedItems.Last());
			_wantedItems.RemoveAt(_wantedItems.Count - 1);
		}

		_currentState = State.IDLE;
	}
}
