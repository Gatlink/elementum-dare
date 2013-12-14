using UnityEngine;	
using System.Collections;

public class MoveToObject : MonoBehaviour {
	
	public float MoveSpeed;
	public float RotateSpeed;

	private Quaternion _rotateTo;
	private Vector3 _target;
	private bool _elevate;
	
	public Transform Target
	{
		set
		{
			_target = value.position;
			_target.y = transform.position.y;
			Reset();
		}
	}

	void Start() {
		enabled = false;

		Reset();
	}

	void Update() {
		if (transform.position == _target)
			enabled = false;

		if (transform.rotation != _rotateTo)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTo, RotateSpeed * Time.deltaTime);
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

	public void Reset()
	{
		Vector3 moveDir = getMoveDirection();
		_rotateTo = Quaternion.LookRotation(moveDir);
	}

	private Vector3 getMoveDirection()
	{
		Vector3 direction = _target - transform.position;
		return direction.normalized;
	}
}
