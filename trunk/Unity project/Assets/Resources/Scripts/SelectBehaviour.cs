﻿using UnityEngine;
using System.Collections;

public class SelectBehaviour : MonoBehaviour {
	public Color OutlineColor;

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
