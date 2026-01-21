using Godot;


[GlobalClass]
public partial class FurnitureData : Resource
{
	[Export]
	PackedScene subScene;
	[Export]
	Vector3 position;
}
