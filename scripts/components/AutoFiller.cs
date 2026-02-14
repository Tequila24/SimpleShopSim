using Godot;
using System;

public partial class AutoFiller : Node
{
	enum ExchangeDirection
	{
		FROM_ATTACHED,
		TO_ATTACHED
	}
	[Export]
	private ExchangeDirection _exchangeDirection;

	[Export]
	private Interactive _interactZone;
	[Export]
	private Timer _interactionTimer;
	[Export]
	private Node _NodeStaticExchanger;
	private IAutoExchange _staticExchanger;

	// private Node _NodeDynamicExchanger;
	private IAutoExchange _dynamicExchanger;


	public override void _Ready()
	{
		_staticExchanger = (IAutoExchange)_NodeStaticExchanger;

		if (_interactionTimer == null)
		{
			_interactionTimer = new Timer();
			_interactionTimer.OneShot = false;
			this.AddChild(_interactionTimer);
			_interactionTimer.WaitTime = 0.3f;
		}


		_interactZone.OnEntered += (Node3D enteredNode) =>
		{
			// _NodeDynamicExchanger = enteredNode;
			// _dynamicExchanger = enteredNode as IAutoExchange;
			_dynamicExchanger = Utils.GetFirstChildOfType<IAutoExchange>(enteredNode);
			
			if (_dynamicExchanger == null)
			{
				GD.Print($"Dynamic exchanger NOT FOUND in node {enteredNode.Name}");
			}
			else
			{
				GD.Print($"Dynamic exchanger FOUND in node {enteredNode.Name}");
			}

			_interactionTimer.Start();
		};
		_interactZone.OnExited += (Node3D unused) =>
		{
			// _NodeDynamicExchanger = null;
			_dynamicExchanger = null;
			_interactionTimer.Stop();
		};

		_interactionTimer.Timeout += DoFill;
	}

	private void DoFill()
	{
		IAutoExchange from = null;
		IAutoExchange to = null;

		if (_exchangeDirection == ExchangeDirection.FROM_ATTACHED)
		{
			from = _staticExchanger;
			to = _dynamicExchanger;
		}
		else
		{
			from = _dynamicExchanger;
			to = _staticExchanger;
		}

		if (from == null || to == null)
		{
			_interactionTimer.Stop();
			return;
		}


		ItemData itemToExchangeData = from.PeekAutoNextItem();
		if (to.TryAutoPutItem(itemToExchangeData))
		{
			var unused = from.TryAutoTakeItem();
		}
	}
}
