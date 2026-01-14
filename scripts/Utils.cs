using Godot;

public partial class Utils
{
	public static T GetFirstChildOfType<T>(Node parent) where T : class
	{
		foreach (var child in parent.GetChildren())
		{
			if (child is T)
				return (T)(object)child;
		}

		return null;
	}

	public static T GetFirstSiblingOfType<T>(Node self) where T : class
	{
		var parent = self.GetParent();
		if (parent == null)
			return null;

		foreach (var sibling in parent.GetChildren())
		{
			if (sibling == self)
				continue;

			if (sibling is T)
				return (T)(object)sibling;
		}

		return null;
	}

	public static T GetParentOfType<T>(Node child) where T : class
	{
		Node nextParent = child.GetParent();
		while (nextParent != null)
		{
			if (nextParent is T parentOfType)
			{
				return parentOfType;
			}

			nextParent = nextParent.GetParent();
		}

		return null;
	}

	public static bool IsNodeChildOrSelfOfParent(Node child, Node parent)
	{
		if (child == parent)
			return true;

		Node nextParent = child.GetParent();
		while (nextParent != null)
		{
			if (nextParent == parent)
				return true;

			nextParent = nextParent.GetParent();
		}

		return false;
	}

	public static double GetTimerValue(Timer timer)
	{
		return ((timer.WaitTime - timer.TimeLeft) / timer.WaitTime);
	}
}