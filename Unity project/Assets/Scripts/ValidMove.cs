using UnityEngine;
using System.Collections;

public class ValidMove : MonoBehaviour {
	public Transform Target;

	public void OnMouseDown() {
		Target.GetComponent<MoveToObject>().enabled = true;
	}
}
