using Godot;


[GlobalClass]
public partial class AccountData : Resource
{
	[Export]
	public int MoneyAmount;
	[Export]
	Godot.Collections.Array<Resource> allBuildings;
}