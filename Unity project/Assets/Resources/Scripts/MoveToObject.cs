using UnityEngine;	
using System.Collections;

public class MoveToObject : MonoBehaviour {

	public Transform Target;
	public float MoveStep;
	public float RotateStep;

	private Quaternion _rotateTo;

	void Start() {
		enabled = false;

		Vector3 moveDir = getMoveDirection();
		_rotateTo = new Quaternion();
		_rotateTo.SetFromToRotation(transform.forward, moveDir);
	}

	void Update() {
		if (transform.position == Target.position)
			enabled = false;

		if (transform.rotation != _rotateTo)
			transform.rotation = Quaternion.Lerp(transform.rotation, _rotateTo, RotateStep * Time.deltaTime);
		else
			transform.position = Vector3.MoveTowards(transform.position, Target.position, MoveStep * Time.deltaTime);
	}

	private Vector3 getMoveDirection()
	{
		Vector3 direction = Target.position - transform.position;
		direction.y = 0;
		return direction.normalized;
	}
}
