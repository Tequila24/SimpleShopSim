using Godot;
using System;

public partial class SuppliesMaster : Node
{
	public static SuppliesMaster Instance
	{ private set; get; }

	[Export]
	private AnimationPlayer _truckAnim;



	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
	}

	public void InitNewDeliveryOrder(ItemCountData itemToDeliver)
	{
	}

	public void SpawnDelivery()
	{
	}

	// _truckAnim.Play("DeliveryArrive");
	// _truckAnim.Play("DeliveryDepart");

}
