using System.Net.Http.Headers;
using Godot;


public partial class AccountWrapper : Node
{
	public static AccountWrapper Instance
	{
		private set; get;
	}

	[Export]
	private AccountData _accountData;

	
	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}

	static public ItemCountData GetAccMoney()
	{
		return Instance._accountData.money;
	}

	static public Godot.Collections.Array<FurnitureData> GetUnlockedFurniture()
	{
		Godot.Collections.Array<FurnitureData> retArray = [];

		foreach (var item in Instance._accountData.furnitureUnlocks)
		{
			if (item.Value)
				retArray.Add(item.Key);
		}

		return retArray;
	}
}
