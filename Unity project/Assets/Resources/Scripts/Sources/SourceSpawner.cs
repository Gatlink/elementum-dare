using UnityEngine;
using System.Collections;

public class SourceSpawner : MonoBehaviour {

	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update () 
	{
		if(!Input.GetKeyDown(KeyCode.Space))
			return;

		if(!Selector.HasTargetSelected("Bloc"))
			return;

		//SpawnSource()
	}
}
