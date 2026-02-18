using Godot;


[GlobalClass]
public partial class ItemData : Resource
{
	[Export]
	public string name;
	[Export]
	public int price;
	[Export]
	public Texture2D image;
	[Export]
	public PackedScene subScene;
}
