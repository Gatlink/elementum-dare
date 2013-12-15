using System.Collections.Generic;

public class IManager<T>
{
	protected static List<T> _items = new List<T>();

	public static void RegisterElement(T elem)
	{
		if(elem != null && !_items.Contains(elem))
			_items.Add(elem);
	}

	public static void UnregisterElement(T elem)
	{
		_items.Remove(elem);
	}
}