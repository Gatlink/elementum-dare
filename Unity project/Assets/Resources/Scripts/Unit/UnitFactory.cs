using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UnitFactory
{
	private static Dictionary<Unit.ETeam, int> _teamId = InitTeamIds();

	public static Unit CreateUnit(Unit.ETeam team, Bloc.BlocType blocType, Source.SourceType sourceType)
	{
		// GAMEOBJECT
		string unitName = string.Format("Unit_{0}_{1}", team, _teamId[team]++);
		GameObject obj = new GameObject(unitName);
		obj.tag = "Unit";
		obj.layer = LayerMask.NameToLayer("Units");

		// MESH
		string meshPath = string.Format("Mesh/Units/{0}/{0}_{1}", team, blocType);
		MeshFilter mesh = obj.AddComponent<MeshFilter>();
		mesh.mesh = Resources.Load<Mesh>(meshPath);

		// MATERIAL
		string materialPath = string.Format ("Mesh/Materials/{0}_{1}", team, sourceType);
		MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
		renderer.material = Resources.Load<Material>(materialPath);
		string texturePath = string.Format("Texture/Units/{0}/{0}_{1}", team, sourceType);
		renderer.material.mainTexture = Resources.Load<Texture>(texturePath);

		// BOX COLLIDER
		BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
		boxCollider.isTrigger = true;

		// RIGIDBODY
		Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;

		// MOVETOOBJECT
		obj.AddComponent<MoveToObject>();

		// SELECTABLE
		obj.AddComponent<Selectable>();

		Unit unit = obj.AddComponent<Unit>();
		unit.Team = team;
		unit.BlocType = blocType;
		unit.SourceType = sourceType;

		return unit;
	}

	private static Dictionary<Unit.ETeam, int> InitTeamIds()
	{
		var dict = new Dictionary<Unit.ETeam, int>();

		dict.Add(Unit.ETeam.Monster, 1);
		dict.Add(Unit.ETeam.Totem, 1);

		return dict;
	}
}
