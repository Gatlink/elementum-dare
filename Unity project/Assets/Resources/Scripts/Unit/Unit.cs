using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour, PhaseEventListener {
	public enum ETeam
	{
		Totem,
		Monster
	}

	public static List<Unit> Units = new List<Unit>();

	public ETeam Team = ETeam.Totem;
	public int Hitpoints= 2;
	public int Moves = 2;
	public Source.SourceType SourceType;
	public Bloc.BlocType BlocType;
	public float MoveSpeed = 150;
	public float RotateSpeed = 400;
	
	private Quaternion _rotateTo;
	private Transform _target = null;
	private Vector3 _relOriginForwardRay;
	private Bloc _bloc;
	private int _currentMoves;

	public Transform Target
	{
		get
		{
			return _target;
		}
		
		set
		{
			_target = value;
			ResetMovement();
		}
	}

	public Bloc CurrentBloc
	{
		get { return _bloc; }
		set
		{
			if (_bloc != null)
				_bloc.WelcomeUnit(null);

			_bloc = value;

			if (_bloc != null)
				_bloc.WelcomeUnit(this);
		}
	}

	void Start ()
	{
		ResetMovement();
	}

	void Update ()
	{
		if (Target != null)
			Move();
	}

	public void MoveToBloc(Bloc bloc)
	{
		BlocIndex positionIndex = bloc.indexInMap;
		positionIndex.z += 1;
		transform.position = Map.IndexToPosition(positionIndex);
		CurrentBloc = bloc;
	}

	public void FaceYourOpponent()
	{
		if (Team == ETeam.Monster)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.Rotate(transform.up, 180);
		}
	}

	public void CreateSource(Bloc target)
	{
		SourceManager.Instance().SpawnSource(SourceType);
	}

	
	void Move() {
		if(!TargetIsAccessible() || CurrentBloc == null)
		{
			Target = null;
			return;
		}
		
		Vector3 vDest = Target.transform.position;
		vDest.y = transform.position.y;
		
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
			_currentMoves = _currentMoves - dest.FlatDistance(_bloc);
			MoveToBloc(dest);
			Target = null;
		}
	}
	
	public void ResetMovement()
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

		return bloc.IsReachable() && bloc.FlatDistance(_bloc) <= _currentMoves;
	}

	public void onEndPhase(int phase) {}
	public void onStartNewPhase(int phase)
	{
		if (_currentMoves < Moves)
			_currentMoves = Moves;
	}
}
