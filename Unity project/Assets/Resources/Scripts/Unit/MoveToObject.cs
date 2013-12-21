using UnityEngine;	
using System.Collections;

public class MoveToObject : MonoBehaviour {
	
	public float MoveSpeed = 150;
	public float RotateSpeed = 400;

	private Quaternion _rotateTo;
	private Transform _target = null;
	private Vector3 _relOriginForwardRay;
	
	public Transform Target
	{
		get
		{
			return _target;
		}

		set
		{
			_target = value;
			Reset();
		}
	}

	void Start() {
		enabled = false;

		Reset();
	}

	void Update() {
		if(!TargetIsAccessible())
			return;

		Vector3 vDest = Target.transform.position;
		vDest.y = transform.position.y;

		Unit unit = gameObject.GetComponent<Unit>();
		if(unit == null || unit.CurrentBloc == null)
		{
			enabled = false;
			return;
		}

		// Face the destination
		if (transform.rotation != _rotateTo)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTo, RotateSpeed * Time.deltaTime);
		else if (transform.position != vDest)
		{
			// Climb up
			Debug.DrawRay(collider.bounds.center, Vector3.down * (5 + collider.bounds.size.y/2), Color.red);
			Debug.DrawRay(transform.position + _relOriginForwardRay, transform.forward * (vDest - transform.position).magnitude, Color.red);
			if (Physics.Raycast(collider.bounds.center, Vector3.down, 5 + collider.bounds.size.y/2)
				|| Physics.Raycast(transform.position + _relOriginForwardRay, transform.forward, (vDest - transform.position).magnitude))
				transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, MoveSpeed * Time.deltaTime);
			// Move toward target
			else
				transform.position = Vector3.MoveTowards(transform.position, vDest, MoveSpeed * Time.deltaTime);
		}
		// Climb down
		else if (!Physics.Raycast(collider.bounds.center, Vector3.down, 0.1f + collider.bounds.size.y/2.0f))
			transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, MoveSpeed * Time.deltaTime);
		// End of the road
		else
		{
			Bloc dest = Target.gameObject.GetComponent<Bloc>();
			unit.MoveToBloc(dest);
			enabled = false;
		}
	}

	public void Reset()
	{
		Vector3 moveDir = getMoveDirection();
		_rotateTo = Quaternion.LookRotation(moveDir);

		// Compute the ray to check for collision in front of the unit
		_relOriginForwardRay = collider.bounds.center - transform.position;
		_relOriginForwardRay.y = _relOriginForwardRay.y - collider.bounds.size.y/2 - 4.9f;
	}

	private Vector3 getMoveDirection()
	{
		Vector3 direction = (Target ? Target.transform.position : Vector3.zero) - transform.position;
		direction.y = 0;
		return direction.normalized;
	}

	private bool TargetIsAccessible()
	{
		//Assume Target is a Bloc
		Bloc bloc = Target.GetComponent<Bloc>();

		if(bloc == null)
			return false;

		return bloc.IsReachable();
	}
}
