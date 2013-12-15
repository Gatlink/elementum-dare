using System.Collections.Generic;

public class IManager<T>
{
	protected List<T> _items = new List<T>();

	public void RegisterElement(T elem)
	{
		if(elem != null && !_items.Contains(elem))
			_items.Add(elem);
	}

	public void UnregisterElement(T elem)
	{
		_items.Remove(elem);
	}
}
