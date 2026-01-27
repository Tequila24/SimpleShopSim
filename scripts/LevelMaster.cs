using Godot;


public partial class LevelMaster : Node
{
	public static LevelMaster Instance
	{ private set; get; }

	[Export]
	public NavigationMaster NavMaster;



	public override void _EnterTree()
	{
		base._EnterTree();

		Instance ??= this;
	}
}
