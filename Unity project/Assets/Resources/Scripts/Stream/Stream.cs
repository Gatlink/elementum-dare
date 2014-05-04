using UnityEngine;
using System.Collections.Generic;

public abstract class Stream : MonoBehaviour
{
	protected Source.SourceType _type; 
	public Source.SourceType Type
	{
		get { return _type; }
	}

	protected Bloc _bloc;
	public Bloc Bloc
	{
		get { return _bloc; }
	}

	///////////////////////////////////////////////////// 
	/// STREAM VALUES
	///////////////////////////////////////////////////// 
	protected int _value;
	public int Value
	{ 
		get {return _value;}
	}

	protected int _buffer;
	public int Buffer
	{
		get { return _buffer; }
		set 
		{
			_buffer = value;
			_hasChanged = true;
		}
	}

	protected bool _hasChanged;
	public bool HasChanged
	{
		get { return _hasChanged; }
	}

	protected bool _resolved;
	public bool Resolved
	{
		get { return _resolved; }
	}

	public void TransmitBuffer()
	{
		//Debug.Log("Transmiting buffer [" + _buffer + "] of " + _type + " for " + _bloc.ToString());
		_value += _buffer;
		_buffer = 0;
		_hasChanged = false;
		_resolved = true;
	}

	public void Generate( int val )
	{	
		_buffer += val;
		_bloc.Streams.Resolve(_type, true);
	}

	public void Erode( int val )
	{ 	
		Debug.Log("Eroding stream for [" + val + "] of " + _type + " for " + _bloc.ToString());
		_value -= Mathf.Max(0, _value - val); 
	}

	public void ResetValues()
	{
		_value = 0;
		_buffer = 0;
		_hasChanged = false;
		_resolved = true;
	}

	public void Trigger(bool state)
	{	_value = state ? 1 : 0;	}

	public void LostInInteraction( int val )
	{	
		//Debug.Log("Interacting [" + val + "] of " + _type + " for " + _bloc.ToString());
		_value = Mathf.Max(0, _value - val); 
	}

	///////////////////////////////////////////////////// 
	/// STREAM SETTINGS
	///////////////////////////////////////////////////// 
	public StreamInfo Settings
	{
		get
		{ return StreamFactory.GetStreamSettingsByType(_type); }
	}

	///////////////////////////////////////////////////// 
	/// STREAM VISUAL
	///////////////////////////////////////////////////// 
	public GameObject Visual
	{
		get { return gameObject; }
	}

	protected bool _visible;
	public bool Visible
	{
		get { return _visible; }
		set
		{
			_visible = value;
			renderer.enabled = _visible;
		}
	}

	public void SetVisible(bool state)
	{ Visible = state; }

	///////////////////////////////////////////////////// 
	/// OTHER & MISC
	///////////////////////////////////////////////////// 
	public new string ToString()
	{
		return _type.ToString() + " : " + "v[" + _value + "] - b[" + _buffer + "] - vis[ " + ((renderer != null && renderer.enabled) ? "X" : " ") + "]";
	}

	public static bool IsFluidType( Source.SourceType streamType )
	{
		return ( streamType == Source.SourceType.Sand
		        || streamType == Source.SourceType.Water
		        || streamType == Source.SourceType.Lava
		        );
	}

	public bool IsFluid
	{
		get { return IsFluidType(_type); }
	}

	// Use this for initialization
	void Start() 
	{
		iTween.Init(gameObject);
	}

	public void Initialize(Bloc parent, Source.SourceType type)
	{
		if(parent == null)
		{
			Debug.Log ("Stream initialized without valid parent bloc!");
			return;
		}
		
		_bloc = parent;
		
		BlocIndex streamIndex = _bloc.indexInMap;
		streamIndex.z += 1;
		//transform.position = Map.IndexToPosition(streamIndex);
		transform.parent = parent.transform;
		transform.localPosition = Vector3.zero;
		
		_type = type;	

		ResetValues();
	}

	public void Clear()
	{
		ResetValues();
		renderer.enabled = false;
		GameObject.Destroy(gameObject);
	}
	
	public int GetAltitude()
	{
		if(_bloc)
			return _bloc.indexInMap.z + 1;
		else
			return -1;
	}
	
	public int GetVolume()
	{
		return _value;
	}
	
	public abstract void UpdateStreamState();
	public abstract void UpdateStreamVisual(bool animated = false);

	public virtual void Update(){}
}
