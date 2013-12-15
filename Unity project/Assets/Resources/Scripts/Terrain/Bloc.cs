using UnityEngine;
using System.Collections;

public class Bloc : MonoBehaviour 
{
	public BlocIndex indexInMap {get; set;}

	public enum BlocType
	{
		TerrainBloc,
		Earth,
		Rock,
		Ice,
		Metal,
		Plant, 
		Upgraded_Plant
	};

	public void InsertedAt(BlocIndex pos)
	{
		gameObject.transform.position = Map.IndexToPosition(pos);
	}


	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update() {}
}
