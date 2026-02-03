using Godot;
using System;


public partial class LevelMaster : Node3D
{
	public static LevelMaster Instance
	{ private set; get; }

	Godot.Collections.Array<ShelfLogic> shelves = [];
	CashRegisterLogic _cashRegister;

	public Action OnLevelReady;


	public override void _EnterTree()
	{
		base._EnterTree();

		Instance ??= this;
	}

	public override void _Ready()
	{
		base._Ready();

		foreach (var child in GetChildren())
		{
			if (child is ShelfLogic newShelf)
			{
				shelves.Add(newShelf);
				continue;
			}

			if (child is CashRegisterLogic cashRegister)
			{
				_cashRegister = cashRegister;
			}
		}

	}


	public void OnSomethingPlaced(Node3D nodePlaced)
	{
		switch (@nodePlaced)
		{
			case ShelfLogic newShelf:
				{
					GD.Print($"New Shelf {newShelf.Name}");
					shelves.Add(newShelf);
					break;
				}
			case CashRegisterLogic newRegister:
				{
					GD.Print($"New Cash Register! {newRegister.Name}");
					_cashRegister = newRegister;
					break;
				}

			default:
				break;
		}
	}

	public ShelfLogic TryFindShelfWithItem(ItemData searchedItem)
	{
		if (shelves.Count <= 0)
			return null;

		foreach (var shelf in shelves)
		{
			if (shelf.DoesHaveItem(searchedItem))
			{
				return shelf;
			}
			else
			{
				continue;
			}
		}

		return null;
	}

	public CashRegisterLogic TryFindCashRegister()
	{
		return _cashRegister;
	}


}
