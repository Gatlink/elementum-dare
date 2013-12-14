using UnityEngine;	
using System.Collections;

public class MoveToObject : MonoBehaviour {

	public Transform Target;
	public float MoveSpeed;
	public float RotateSpeed;

	private Quaternion _rotateTo;
	private Vector3 _target;
	private bool _elevate;

	void Start() {
		enabled = false;

		Reset();
	}

	void Update() {
		if (transform.position == _target)
			enabled = false;

		if (transform.rotation != _rotateTo)
			transform.rotation = Quaternion.Lerp(transform.rotation, _rotateTo, RotateSpeed * Time.deltaTime);
		else if (_elevate)
		{
			transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, MoveSpeed * Time.deltaTime);
			_target.y = transform.position.y;
		}
		else
			transform.position = Vector3.MoveTowards(transform.position, _target, MoveSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		_elevate = true;
	}

	void OnTriggerExit(Collider other)
	{
		_elevate = false;
	}

	void Reset()
	{
		Vector3 moveDir = getMoveDirection();
		_rotateTo = new Quaternion();
		_rotateTo.SetFromToRotation(transform.forward, moveDir);
		
		_target = Target.position;
		_target.y = transform.position.y;
	}

	private Vector3 getMoveDirection()
	{
		Vector3 direction = Target.position - transform.position;
		direction.y = 0;
		return direction.normalized;
	}
}
