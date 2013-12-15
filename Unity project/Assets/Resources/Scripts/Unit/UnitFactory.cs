using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UnitFactory
{
	private static Dictionary<Unit.Teams, int> _teamId = null;

	public static Unit CreateUnit(Unit.Teams team, string bloc, string source)
	{
		if (_teamId == null)
		{
			_teamId = new Dictionary<Unit.Teams, int>();
			_teamId.Add(Unit.Teams.Monstre, 1);
			_teamId.Add(Unit.Teams.Totem, 1);
		}

		// GAMEOBJECT
		string unitName = string.Format("Unit_{0}_{1}", team, _teamId[team]++);
		GameObject obj = new GameObject(unitName);
		obj.tag = "Unit";
		obj.layer = LayerMask.NameToLayer("Units");

		// MESH
		string meshPath = string.Format("Mesh/Units/{0}/{0}_{1}", team, bloc);
		MeshFilter mesh = obj.AddComponent<MeshFilter>();
		mesh.mesh = Resources.Load<Mesh>(meshPath);

		// MATERIAL
		string materialPath = string.Format ("Mesh/Materials/{0}_{1}", team, source);
		MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
		renderer.material = Resources.Load<Material>(materialPath);

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

		return unit;
	}
}
