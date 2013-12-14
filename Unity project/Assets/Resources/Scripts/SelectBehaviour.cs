using UnityEngine;
using System.Collections;

public class SelectBehaviour : MonoBehaviour {
	public Color DisabledColor = new Color(55,55,55);
	public Color ActiveColor = new Color(235,235,235);

	private bool _isSelected;
	public bool IsSelected
	{
		get { return _isSelected; }
		set
		{
			_isSelected = value;
			renderer.material.color = value ? ActiveColor : DisabledColor;
		}
	}

	void Start()
	{
		IsSelected = false;
	}

	void Select()
	{
		IsSelected = true;
	}

	void Unselect()
	{
		IsSelected = false;
	}
}
