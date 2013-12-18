using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlocFactory
{
	private static int blocID = 0;

	private static Dictionary<Bloc.BlocType, BlocInfo> blocInfoByType = new Dictionary<Bloc.BlocType, BlocInfo>();

	public static Bloc CreateBloc(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		BlocInfo bloc = blocInfoByType[type];
		
		if(!bloc)
		{
			Debug.LogError("Creating a bloc of an unknown type. [" + type.ToString() + "]");
			return null;
		}

		GameObject blocObj = CreateObjectFromBlocInfo(bloc);
		blocObj.tag = "Bloc";
		blocObj.layer = LayerMask.NameToLayer("Terrain");
		
		if(!blocObj)
		{
			Debug.LogError("Error creating the bloc. [" + type.ToString() + "]");
			return null;
		}
		
		return blocObj.GetComponent<Bloc>();
	}

	public static void RegisterBlocInfo(BlocInfo info)
	{
		blocInfoByType.Add(info.type, info);
	}
	
	private static GameObject CreateObjectFromBlocInfo(BlocInfo bloc)
	{
		GameObject obj = new GameObject("Bloc #" + blocID++);
		
		//Need a mesh filter and a mesh renderer for the stream's mesh rendering
		MeshFilter filter = obj.AddComponent("MeshFilter") as MeshFilter;
		filter.mesh = Object.Instantiate(bloc.mesh) as Mesh;
		
		MeshRenderer renderer = obj.AddComponent("MeshRenderer") as MeshRenderer;
		renderer.material = Object.Instantiate(bloc.material) as Material;
		
		//Add a box collider
		MeshCollider hitBox = obj.AddComponent("MeshCollider") as MeshCollider;
		hitBox.transform.parent = obj.transform;
		
		//Add proper stream script
		obj.AddComponent("Selectable");
		obj.AddComponent("Bloc");
		
		return obj;
	}

	public static Vector3 GetBlocSize(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		return blocInfoByType[type].mesh.bounds.size;
	}

	public static bool IsReady()
	{
		return blocInfoByType.Count >= Bloc.NB_OF_TYPES;
	}
}
