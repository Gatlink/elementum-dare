using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlocFactory : MonoBehaviour
{
	private static int _blocID = 0; 

    public GameObject[] BlocReferences;

	public Bloc CreateBloc(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		GameObject blocObj = GameObject.Instantiate(_instance.BlocReferences[(int)type]) as GameObject;
		blocObj.name = "Bloc #" + _blocID++ + " : " + type.ToString();

		if(!blocObj)
		{
			Debug.LogError("Error creating the bloc. [" + type.ToString() + "]");
			return null;
		}

		return blocObj.GetComponent<Bloc>();
	}
	
	public Vector3 GetBlocSizeByType(Bloc.BlocType type)
	{
		MeshFilter mf = BlocReferences[(int)type].GetComponent<MeshFilter>();

		if(mf != null)
			return mf.sharedMesh.bounds.size;
		else
			return Vector3.zero;
	}

	public Vector3 GetBlocSize()
	{
		return GetBlocSizeByType(Bloc.BlocType.TerrainBloc);
	}

	//Singleton
	private static BlocFactory _instance = null;
	public static BlocFactory Instance
	{ get { return _instance; } }

	//MonoBehaviour
	public void Awake() 
	{
		_instance = this;
	}
}
