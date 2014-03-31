using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour, PhaseEventListener
{

#region Static

	// All spawned units
	public static List<Unit> Units = new List<Unit>();

	public static IEnumerable<Unit> Monsters
	{
		get { return Units.Where(unit => unit.Team == ETeam.Monster); }
	}

	public static IEnumerable<Unit> Totems
	{
		get { return Units.Where(unit => unit.Team == ETeam.Totem); }
	}

	public static void CleanDeadUnits()
	{
		List<Unit> deads = Units.Where(unit => unit.IsDead).ToList();
		foreach (Unit unit in deads)
			unit.Die();
	}

	private static Material brokenMaterialTotem;
	private static Material brokenMaterialMonster;

#endregion

#region  Teams

	public enum ETeam
	{
		Totem,
		Monster
	}

#endregion

#region Max Attributes

	public const int HitPointsMax= 2;
	public const int MovesMax = 2;

#endregion

#region Attributes

	public bool HasActed;
	public ETeam Team = ETeam.Totem;
	public Source.SourceType SourceType;
	public Bloc.BlocType BlocType;

	private int _hitPoints = HitPointsMax;
	private int _moves;

#endregion

#region Movement

	#region variables

	public float MoveSpeed = 150;
	public float RotateSpeed = 400;
	public BlocAccessor Accessor = new BlocAccessor(Color.red);

	private Quaternion _rotateTo;
	private Vector3 _relOriginForwardRay;
	private List<Bloc> _currentPath = new List<Bloc>();

	#endregion

	#region Properties

	public Bloc Target
	{
		get
		{
			return _currentPath.FirstOrDefault();
		}
		
		set
		{
			if (!value || _moves == 0
			    || (Accessor.AccessibleBlocs != null && !Accessor.AccessibleBlocs.Contains(value)))
			{
				_currentPath.Clear();
				return;
			}

			_currentPath = Accessor.GetPath(_bloc, value);
			ResetMovement();
		}
	}

	private Bloc _bloc;
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

	#endregion

	#region Methods

	public void FaceYourOpponent()
	{
		if (Team == ETeam.Monster)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.Rotate(transform.up, 180);
		}
	}

	public void MoveToBloc(Bloc bloc)
	{
		BlocIndex positionIndex = bloc.indexInMap;
		positionIndex.z += 1;
		transform.position = Map.IndexToPosition(positionIndex);
		CurrentBloc = bloc;
	}

	private void Move() {
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
			_moves = _moves - Target.FlatDistance(_bloc);
			MoveToBloc(Target);
			_currentPath.Remove(Target);
			Accessor.Clear();
			ResetMovement();
		}
	}

	public void ResetMovement()
	{
		Vector3 moveDir = GetMoveDirection();
		_rotateTo = Quaternion.LookRotation(moveDir);
		
		// Compute the ray to check for collision in front of the unit
		_relOriginForwardRay = collider.bounds.center - transform.position;
		_relOriginForwardRay.y = _relOriginForwardRay.y - collider.bounds.size.y/2 - 4.9f;
	}
	
	private Vector3 GetMoveDirection()
	{
		Vector3 direction = (Target ? Target.transform.position : Vector3.zero) - transform.position;
		direction.y = 0;
		return direction.normalized;
	}

	public void UpdateAccessibleBlocs()
	{
		Accessor.Update(_bloc, _moves);
	}

	#endregion

#endregion

#region Monobehaviour

	void Awake()
	{
		if (brokenMaterialTotem == null)
			brokenMaterialTotem = Resources.Load<Material>("Mesh/Materials/Totem_Broken");
	}

	void Update()
	{
		if (Selector.Selected == this)
		{
			if (Accessor.AccessibleBlocs == null)
				UpdateAccessibleBlocs();
		}

		if (Target != null)
			Move();
	}

#endregion

#region Gameplay

	public bool IsDead
	{
		get { return _hitPoints <= 0; }
	}

	public Source CreateSource()
	{
		return SourceManager.Instance.SpawnSource(SourceType);
	}

	public Bloc CreateBloc()
	{
		return BlocFactory.CreateBloc(BlocType);
	}

	private void Die()
	{
		if (Selector.Selected == collider)
			Selector.Selected = null;
		GameTicker.UnregisterListener(this);
		Units.Remove(this);
		_bloc.WelcomeUnit(null);
		Destroy(gameObject);
	}

#endregion

#region PhaseEventListener

	public void onEndPhase(int phase)
	{
		Accessor.Clear();
		Target = null;
	}

	public void onStartNewPhase(int phase)
	{
		_moves = MovesMax;
		HasActed = false;

		if (_bloc.IsUnderLava || _bloc.IsElectrified || _bloc.Type == Bloc.BlocType.UpgradedPlant)
		{
			_hitPoints -= 1;
			if (renderer.materials.Count() <= 1)
			{
				Material[] materials = new Material[2];
				materials[0] = renderer.material;
				materials[1] = Team == ETeam.Totem ? brokenMaterialTotem : brokenMaterialMonster;
				renderer.materials = materials;
			}
		}

		// if (_bloc.HasWindBlowing)
	}

#endregion

}
