using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Bloc : MonoBehaviour 
{
	public enum BlocType
	{
		TerrainBloc,
		Earth,
		Rock,
		Ice,
		Metal,
		Plant, 
		UpgradedPlant
	};
	public static int NB_OF_TYPES = (int)BlocType.UpgradedPlant;
	// must be last of enum

	private StreamCollection _streams = new StreamCollection();
	public StreamCollection Streams { get{ return _streams; } }

	public bool HasStreamOfType(Source.SourceType type)
	{	return Streams[type].GetVolume() > 0;	}

	public bool HasPendingStreamOfType(Source.SourceType type)
	{	return Streams[type].HasChanged; }

	public bool HasPendingStreamChanges()
	{	return Streams.HasChanged;	}

	public bool IsFlooded
	{
		get{ return HasStreamOfType(Source.SourceType.Water); }
	}
	
	public bool IsQuickSanded
	{
		get{ return HasStreamOfType(Source.SourceType.Sand); }
	}
	
	public bool IsUnderLava
	{
		get{ return HasStreamOfType(Source.SourceType.Lava); }
	}
	
	public bool IsElectrified 
	{
		get{ return HasStreamOfType(Source.SourceType.Electricity); } 
		set{ Streams[Source.SourceType.Electricity].Trigger(_type == BlocType.Metal || IsFlooded && value); }
	}

	public bool HasWindBlowing
	{
		get{ return HasStreamOfType(Source.SourceType.Wind); } 
		set{ Streams[Source.SourceType.Wind].Trigger(value); }
	}

	public bool IsConductor()
	{	return (_type == Bloc.BlocType.Metal) || IsFlooded;	}

	private BlocType _type;
	public BlocType Type 
	{ 
		get{ return _type; }
		set
		{
			if (value != _type)
			{
				_type = value;
				renderer.material = Resources.Load<Material>(string.Format("Mesh/Materials/Bloc_{0}", value));
			}
		}
	}

	public BlocIndex indexInMap {get; private set;}

	private Source _sourceObject = null;
	public Source Source
	{
		get { return _sourceObject; }
		set { _sourceObject = value;}
	}

	private Unit _unitObject = null;

	public bool HoldASource()
	{	return _sourceObject != null;	}

	public bool HostAUnit()
	{	return _unitObject != null;	}

	public bool WelcomeUnit(Unit unit)
	{	return _unitObject = unit;	}

	public void InsertedAt(BlocIndex pos)
	{
		indexInMap = pos;
		//Debug.Log(pos.ToString());
		transform.position = Map.IndexToPosition(pos);
		//Debug.Log(transform.position);
		transform.parent = Map.GetMapRefTransform();
        gameObject.transform.FindChild("GUI").gameObject.transform.position = transform.position;
	}

	public bool IsReachable()
	{
		if(HoldASource() || HostAUnit())
			return false;

		return true;
	}

	public static bool IsLower(Bloc a, Bloc b)
	{	return (a.indexInMap.z < b.indexInMap.z );	}

	public bool IsLowerThan(Bloc other)
	{	return (indexInMap.z < other.indexInMap.z );}

	public static bool IsHigher(Bloc a, Bloc b)
	{	return (a.indexInMap.z > b.indexInMap.z );	}

	public bool IsHigherThan(Bloc other)
	{	return (indexInMap.z > other.indexInMap.z );}

	// Use this for initialization
	void Awake()
	{
		Streams.Initialize(this);
	}
	
	// Update is called once per frame
	void Update() 
	{}

	public new string ToString()
	{
		string blocIdentifier = gameObject.name.ToUpper() + ":";
		string blocCoords = "COORDS: " + indexInMap.ToString();
		string blocType = "TYPE: " + Type.ToString();
		//string blocState = string.Format("STATE:\nElec: {0}\nWind: {1}", IsElectrified, HasWindBlowing);
		string blocStreams = Streams.ToString();
		string msg = string.Format ("{0}\n{1}\n{2}\n{3}\n", blocIdentifier, blocCoords, blocType, blocStreams);
		return msg;
	}

	public int FlatDistance(Bloc other)
	{
		BlocIndex othIdx = other.indexInMap;
		var distance = Math.Abs(othIdx.x - indexInMap.x) + Math.Abs(othIdx.y - indexInMap.y);
		return distance;
	}

	public bool IsTopMostBloc()
	{
		return Map.GetBlocAt(indexInMap.x, indexInMap.y) == this;
	}

	public static Vector3 GetBlocSizeByType(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		return BlocFactory.Instance.GetBlocSizeByType(type);
	}

	public Vector3 GetBlocSize()
	{
		return BlocFactory.Instance.GetBlocSizeByType(_type);
	}

#region GUI

	public void Highlight(Color color)
	{
        var gui = gameObject.transform.FindChild("GUI").gameObject;
		gui.SetActive(true);
		gui.renderer.material.color = color;
	}

	public void RemoveHighlight()
	{
        gameObject.transform.FindChild("GUI").gameObject.SetActive(false);
	}
	
#endregion
}