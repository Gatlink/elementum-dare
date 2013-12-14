using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class BlocFactory 
{
	private static int blocID = 0;

	static public GameObject CreateBloc(Vector3 pos = default(Vector3))
	{
		//TODO implement with type and all
		GameObject bloc = new GameObject("Bloc #" + blocID++);
		bloc.transform.position = pos;

		MeshFilter filter = bloc.AddComponent("Mesh Filter") as MeshFilter;
		filter.mesh = new Mesh();
		//filter.mesh.

		bloc.AddComponent("Bloc");

		BoxCollider hitBox = bloc.collider as BoxCollider;
		hitBox.center = pos;

		// TODO add a rigidBody?
		return bloc;
	}
}
