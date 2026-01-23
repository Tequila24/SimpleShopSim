using Godot;


[GlobalClass]
public partial class FurnitureData : Resource
{
	[Export]
	public string name;
	[Export]
	public int price;
	[Export]
	public Texture2D image2D;
	[Export]
	public PackedScene scene3D;
}
