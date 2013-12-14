using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class BlocFactory 
{
	static public GameObject CreateBloc(Vector3 pos = default(Vector3))
	{
		//TODO implement with type and all
		GameObject bloc = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bloc.transform.position = pos;
		bloc.transform.localScale = Bloc.BLOC_SIZE;
		bloc.AddComponent("Bloc");

		BoxCollider hitBox = bloc.collider as BoxCollider;
		hitBox.size = Bloc.BLOC_SIZE;
		hitBox.center = pos;

		// TODO add a rigidBody?
		return bloc;
	}
}
