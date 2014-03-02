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

	public static bool operator ==(BlocIndex left, BlocIndex right)
	{
		return (left.x == right.x) && (left.y == right.y) && (left.z == right.z);
	}

	public static bool operator !=(BlocIndex left, BlocIndex right)
	{
		return !(left == right);
	}

	public override bool Equals(System.Object other)
	{
		if (other == null || this.GetType() != other.GetType()) return false;

		BlocIndex right = (BlocIndex)other;
		return (this.x == right.x) && (this.y == right.y) && (this.z == right.z);
	}

	public override int GetHashCode()
	{
		return 1;
	}

	public new string ToString()
	{
		return string.Format ("({0}, {1}, {2})", x, y, z);
	}
}

public class Map 
{
	private static List<Bloc>[,] _internalMap;

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

		_internalMap = new List<Bloc>[width, length];

		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < length; ++y)
			{
				_internalMap[x,y] = new List<Bloc>();
			}
		}
	}

	public static void InsertBloc(int x, int y, Bloc bloc)
	{
		if(bloc != null)
		{
			int z = _internalMap[x,y].Count;

			bloc.InsertedAt(new BlocIndex(x,y,z));

			_internalMap[x,y].Add(bloc);

			bloc.gameObject.layer = LayerMask.NameToLayer("Terrain");
		}
	}

	public static void RemoveBloc(int x, int y)
	{
		//TODO pop bloc and remove from parenting hierarchy
	}

	public static Vector3 IndexToPosition(int x, int y, int z)
	{
		Vector3 size = BlocFactory.GetBlocSize();
		return new Vector3(size.x * x, size.y * z, size.z * y);
	}

	public static Vector3 IndexToPosition(BlocIndex index)
	{
		return IndexToPosition(index.x, index.y, index.z);
	}

	public static Vector3 DimensionRatioToPosition(float xRatio, float yRatio)
	{
		Vector3 size = BlocFactory.GetBlocSize();
		return new Vector3(size.x * xRatio, 0.0f, size.z * yRatio);
	}

	public static Vector3 Get2DMapCenter()
	{
		return DimensionRatioToPosition(_width * 0.5f, _length * 0.5f); 
	}

	public static List<Bloc> FetchNeighbors(BlocIndex index, int range, bool volumetricSearch = false, bool includeStartBloc = false)
	{
		List<Bloc> list = new List<Bloc>();

		int minX = Mathf.Max(0, index.x - range);
		int maxX = Mathf.Min(_width-1, index.x + range);
		int minY = Mathf.Max(0, index.y - range);
		int maxY = Mathf.Min(_length-1, index.y + range);
		int minZ = index.z;
		int maxZ = index.z;

		if(volumetricSearch)
		{
			minZ = Mathf.Max(0, index.z - range);
			maxZ = index.z + range;
		}

		for(int x = minX; x <= maxX; ++x)
		{
			for(int y = minY; y <= maxY; ++y)
			{
				for(int z = minZ; z <= maxZ; ++z)
				{
					/*if(x != index.x && y != index.y) //discard corners
						continue;*/

					Bloc bloc = GetBlocAt(x, y, z);

					if(bloc == null)
						continue;

					if(bloc.indexInMap == index && !includeStartBloc) //discard starting bloc
						continue;
					
					list.Add(bloc);
				}
			}
		}

		return list;
	}

	public static List<Bloc> FetchNeighbors(Bloc bloc, int range, bool volumetricSearch = false, bool includeStartBloc = false)
	{
		return FetchNeighbors(bloc.indexInMap, range, volumetricSearch, includeStartBloc);
	}

	public static List<Bloc> FetchNeighbors2D(BlocIndex index, int range, bool includeStartBloc = false)
	{
		List<Bloc> list = new List<Bloc>();
		
		int minX = Mathf.Max(0, index.x - range);
		int maxX = Mathf.Min(_width-1, index.x + range);
		int minY = Mathf.Max(0, index.y - range);
		int maxY = Mathf.Min(_length-1, index.y + range);
		
		for(int x = minX; x <= maxX; ++x)
		{
			for(int y = minY; y <= maxY; ++y)
			{
				if(x != index.x && y != index.y)
					continue;

				Bloc bloc = GetBlocAt(x, y);//get the one on top
					
				if(bloc == null)
					continue;
				
				if(bloc.indexInMap == index && !includeStartBloc) //discard starting bloc
					continue;
				
				list.Add(bloc);
			}
		}
		
		return list;
	}

	public static List<Bloc> FetchNeighbors2D(Bloc bloc, int range, bool includeStartBloc = false)
	{
		return FetchNeighbors2D(bloc.indexInMap, range, includeStartBloc);
	}

	public static List<Bloc> FetchNeighborsIf(BlocIndex index, int range, System.Func<Bloc, bool> _func, bool volumetricSearch = false, bool includeStartBloc = false)
	{
		List<Bloc> list = new List<Bloc>();
		
		int minX = Mathf.Max(0, index.x - range);
		int maxX = Mathf.Min(_width-1, index.x + range);
		int minY = Mathf.Max(0, index.y - range);
		int maxY = Mathf.Min(_length-1, index.y + range);
		int minZ = index.z;
		int maxZ = index.z;
		
		if(volumetricSearch)
		{
			minZ = Mathf.Max(0, index.z - range);
			maxZ = index.z + range;
		}
		
		for(int x = minX; x <= maxX; ++x)
		{
			for(int y = minY; y <= maxY; ++y)
			{
				for(int z = minZ; z <= maxZ; ++z)
				{					
					Bloc bloc = GetBlocAt(x, y, z);
					
					if(bloc == null)
						continue;
					
					if(bloc.indexInMap == index && !includeStartBloc) //discard starting bloc
						continue;

					if(_func(bloc))
						list.Add(bloc);
				}
			}
		}
		
		return list;
	}
	
	public static List<Bloc> FetchNeighborsIf(Bloc bloc, int range, System.Func<Bloc, bool> _func , bool volumetricSearch = false, bool includeStartBloc = false)
	{
		return FetchNeighborsIf(bloc.indexInMap, range, _func, volumetricSearch, includeStartBloc);
	}

	public static Bloc GetBlocAt(int x, int y, int z = -1)
	{
		int top = _internalMap[x,y].Count-1;

		if(z == -1)
			return (top < 0) ? null :_internalMap[x,y][top];
			//should not be able to return null there

		if(z < 0 || z > top)
			return null;

		return _internalMap[x,y][z];
	}
}
