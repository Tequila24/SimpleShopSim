using Godot;
using System;

public partial class ObjectPopup : Node
{
	[Export]
	Godot.Collections.Dictionary<string, PackedScene> _popups;

	[Export]
	private Node3D _popupHolder
	{
		get { return _popupHolder ?? GetParent<Node3D>(); }
		set { _popupHolder = value; }
	}


	public bool ShowPopupName(string nameKey)
	{
		if (IsAlreadyUp(nameKey))
			return false;

		if (!_popups.ContainsKey(nameKey))
			return false;	

		var newPopup = _popups[nameKey].Instantiate<Control>();
		_popupHolder.AddChild(newPopup);
		return true;
	}

	public bool RemovePopup(string nameKey)
	{
		if (_popupHolder.FindChild(nameKey, false) is Node n) {
			_popupHolder.RemoveChild(n);
			return true;
		} else
		{
			return false;
		}
	}

	public bool IsAlreadyUp(string nameKey)
	{
		return _popupHolder.FindChild(nameKey, false) == null ? false : true;
	}

}
