using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
	private Stack<GameObject>[,] _internalMap;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize(int width, int length)
	{
		_internalMap = new Stack<GameObject>[width, length];

		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < length; ++y)
			{
				_internalMap[x,y] = new Stack<GameObject>();
			}
		}
	}

	public void InsertBloc(int x, int y, GameObject bloc)
	{
		if(bloc != null)
		{
			_internalMap[x,y].Push(bloc);
			bloc.transform.position = IndexToPosition(x,y,_internalMap[x,y].Count-1);
			bloc.transform.parent = transform;
		}
	}

	public void RemoveBloc(int x, int y)
	{
		//TODO pop bloc and remove from parenting hierarchy
	}

	public static Vector3 IndexToPosition(int x, int y, int z)
	{
		return new Vector3(Bloc.BLOC_SIZE.x * -y, Bloc.BLOC_SIZE.y * z, Bloc.BLOC_SIZE.z * x);
	}
}
