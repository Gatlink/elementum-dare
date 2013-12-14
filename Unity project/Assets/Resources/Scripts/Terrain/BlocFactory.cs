using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlocFactory
{
	private static int blocID = 0;

	private static Dictionary<Bloc.BlocType, Material> materialsByType = CreateMaterialsDictionnary();

	private static GameObject defaultCube = CreateDefaultCube();

	public static GameObject CreateBloc(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		GameObject bloc = Object.Instantiate(defaultCube) as GameObject;

		bloc.SetActive(true);
		bloc.name = "Bloc #" + blocID++;
		bloc.tag = "Bloc";
		bloc.layer = LayerMask.NameToLayer("Terrain");

		bloc.transform.position = Vector3.zero;
		bloc.transform.rotation = Quaternion.identity;

		MeshRenderer renderer = bloc.GetComponent("MeshRenderer") as MeshRenderer;

		renderer.material = materialsByType[type];

		return bloc;
	}

	private static GameObject CreateDefaultCube()
	{
		GameObject obj = Object.Instantiate(Resources.Load("Mesh/Bloc_01")) as GameObject;
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;

		MeshCollider col = obj.GetComponent("MeshCollider") as MeshCollider;
		if(col)
			Object.DestroyImmediate(col);

		Animator anim = obj.GetComponent("Animator") as Animator;
		if(anim)
			Object.DestroyImmediate(anim);

		BoxCollider hitBox = obj.AddComponent("BoxCollider") as BoxCollider;
		hitBox.transform.parent = obj.transform;
		hitBox.transform.position = obj.transform.position;

		obj.AddComponent("Bloc");

		obj.SetActive(false);
		obj.name = "Default Bloc";
		obj.tag = "Default Bloc";
		obj.layer = LayerMask.NameToLayer("Game Start Objects");

		return obj;
	}

	private static Dictionary<Bloc.BlocType, Material> CreateMaterialsDictionnary()
	{
		Dictionary<Bloc.BlocType, Material> tmpDictionnary = new Dictionary<Bloc.BlocType, Material>();

		tmpDictionnary.Add(Bloc.BlocType.TerrainBloc, Resources.Load("Mesh/Materials/Bloc_Terre", typeof(Material)) as Material);
		tmpDictionnary.Add(Bloc.BlocType.Earth, Resources.Load("Mesh/Materials/Bloc_Herbe", typeof(Material)) as Material);
		tmpDictionnary.Add(Bloc.BlocType.Rock, Resources.Load("Mesh/Materials/Bloc_Pierre", typeof(Material)) as Material);
		tmpDictionnary.Add(Bloc.BlocType.Ice, Resources.Load("Mesh/Materials/Bloc_Glace", typeof(Material)) as Material);
		tmpDictionnary.Add(Bloc.BlocType.Metal, Resources.Load("Mesh/Materials/Bloc_Metal", typeof(Material)) as Material);
		tmpDictionnary.Add(Bloc.BlocType.Plant, Resources.Load("Mesh/Materials/Bloc_Plante", typeof(Material)) as Material);
		tmpDictionnary.Add(Bloc.BlocType.Upgraded_Plant, Resources.Load("Mesh/Materials/Bloc_Ronces", typeof(Material)) as Material);

		return tmpDictionnary;
	}

	public static Vector3 GetBlocSize()
	{
		BoxCollider hitBox = defaultCube.GetComponent("BoxCollider") as BoxCollider;
		return hitBox.size;
	}
}
