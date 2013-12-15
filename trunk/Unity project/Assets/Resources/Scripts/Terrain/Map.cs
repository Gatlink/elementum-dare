using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct BlocIndex
{
	public BlocIndex(int _x, int _y, int _z)
	{
		x = _x;
		y = _y;
		z = _z;
	}

	public int x {get; set;}
	public int y {get; set;}
	public int z {get; set;}
}

public class Map 
{
	private static Stack<Bloc>[,] _internalMap;

	private static int _width;
	private static int _length;

	private static GameObject _mapRef = null;

	public static Transform GetMapRefTransform()
	{
		if(_mapRef == null)
			_mapRef = GameObject.Find("Terrain");

		return _mapRef.transform;
	}

	public static void Initialize(int width, int length)
	{
		_width = width;
		_length = length;

		_internalMap = new Stack<Bloc>[width, length];

		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < length; ++y)
			{
				_internalMap[x,y] = new Stack<Bloc>();
			}
		}
	}

	public static void InsertBloc(int x, int y, Bloc bloc)
	{
		if(bloc != null)
		{
			int z = _internalMap[x,y].Count;

			bloc.InsertedAt(new BlocIndex(x,y,z));

			_internalMap[x,y].Push(bloc);
		}
	}

	public static void RemoveBloc(int x, int y)
	{
		//TODO pop bloc and remove from parenting hierarchy
	}

	public static Vector3 IndexToPosition(int x, int y, int z)
	{
		Vector3 size = BlocFactory.GetBlocSize();
		return new Vector3(size.x * -y, size.y * z, size.z * x);
	}

	public static Vector3 IndexToPosition(BlocIndex index)
	{
		return IndexToPosition(index.x, index.y, index.z);
	}

	public static Vector3 DimensionRatioToPosition(float xRatio, float yRatio)
	{
		Vector3 size = BlocFactory.GetBlocSize();
		return new Vector3(size.x * -yRatio, 0.0f, size.z * xRatio);
	}

	public static Vector3 Get2DMapCenter()
	{
		return DimensionRatioToPosition(_width * 0.5f, _length * 0.5f); 
	}
}
