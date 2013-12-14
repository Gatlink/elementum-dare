using UnityEngine;	
using System.Collections;

public class MoveToObject : MonoBehaviour {
	
	public float MoveSpeed;
	public float RotateSpeed;

	private Quaternion _rotateTo;
	private Vector3 _target;
	private Vector3 _relOriginForwardRay;
	
	public Transform Target
	{
		set
		{
			_target = value.position;
			Reset();
		}
	}

	void Start() {
		enabled = false;

		Reset();
	}

	void Update() {
		Vector3 vDest = _target;
		vDest.y = transform.position.y;

		// Face the destination
		if (transform.rotation != _rotateTo)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTo, RotateSpeed * Time.deltaTime);
		else if (transform.position != vDest)
		{
			// Climb up
			if (Physics.Raycast(transform.position + _relOriginForwardRay, transform.forward, vDest.magnitude))
				transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, MoveSpeed * Time.deltaTime);
			// Move toward target
			else
				transform.position = Vector3.MoveTowards(transform.position, vDest, MoveSpeed * Time.deltaTime);
		}
		// Climb down
		else if (!Physics.Raycast(collider.bounds.center + Vector3.down * (collider.bounds.size.y/2), Vector3.down, 1f))
			transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, MoveSpeed * Time.deltaTime);
		// End of the road
		else
			enabled = false;
	}

	public void Reset()
	{
		Vector3 moveDir = getMoveDirection();
		_rotateTo = Quaternion.LookRotation(moveDir);

		// Compute the ray to check for collision in front of the unit
		_relOriginForwardRay = collider.bounds.center - transform.position;
		_relOriginForwardRay.z = _relOriginForwardRay.z + collider.bounds.size.z/2;
		_relOriginForwardRay.y = _relOriginForwardRay.y - collider.bounds.size.y/2 - 5;
	}

	private Vector3 getMoveDirection()
	{
		Vector3 direction = _target - transform.position;
		direction.y = 0;
		return direction.normalized;
	}
}
