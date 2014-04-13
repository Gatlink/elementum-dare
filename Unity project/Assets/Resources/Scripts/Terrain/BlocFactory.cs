using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlocFactory : MonoBehaviour
{
    public GameObject[] BlocReferences;

	public Bloc CreateBloc(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		GameObject blocObj = (GameObject) UnityEngine.Object.Instantiate(_instance.BlocReferences[(int)type]);
		blocObj.tag = "Bloc";
		
		if(!blocObj)
		{
			Debug.LogError("Error creating the bloc. [" + type.ToString() + "]");
			return null;
		}
		
		return blocObj.GetComponent<Bloc>();
	}
	
	public Vector3 GetBlocSizeByType(Bloc.BlocType type)
	{
		return BlocReferences[(int)type].GetComponent<MeshFilter>().mesh.bounds.size;
	}

	public Vector3 GetBlocSize()
	{
        return BlocReferences[(int)Bloc.BlocType.TerrainBloc].GetComponent<MeshFilter>().mesh.bounds.size;
	}

	//Singleton
	private static BlocFactory _instance = null;
	public static BlocFactory Instance
	{ get { return _instance; } }

	//MonoBehaviour
	public void Start() 
	{
		_instance = this;
	}
}
