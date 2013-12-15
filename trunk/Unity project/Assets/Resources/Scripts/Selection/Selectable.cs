using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {
	public Color OutlineColor = Color.green;
	public float OutlineWidth = .005f;

	private Shader _selectedShader;
	private Shader _oldShader;

	private bool _isSelected;
	public bool IsSelected
	{
		get { return _isSelected; }
		set
		{
			_isSelected = value;
			renderer.material.shader = _isSelected ? _selectedShader : _oldShader;
		}
	}

	void Start()
	{
		_selectedShader = Shader.Find("Toon/Basic Outline");
		OutlineWidth = Mathf.Clamp(OutlineWidth, .002f, .03f);
		renderer.material.SetColor("_OutlineColor", OutlineColor);
		renderer.material.SetFloat("_Outline", OutlineWidth);

		_oldShader = renderer.material.shader;
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
