using UnityEngine;	
using System.Collections;

public class MoveToObject : MonoBehaviour {

	public Transform Target;
	public float Step;

	void Start () {
		enabled = false;
	}

	void Update () {
		if (transform.position == Target.position)
			enabled = false;

		transform.position = Vector3.MoveTowards(transform.position, Target.position, Step);
	}
}
