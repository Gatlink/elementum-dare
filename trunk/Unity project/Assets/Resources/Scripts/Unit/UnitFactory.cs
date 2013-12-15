using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UnitFactory
{
	private static Dictionary<Unit.ETeam, int> _teamId = InitTeamIds();
	private static Dictionary<string, Source.SourceType> _sourceTypes = InitSourceTypes();
	private static Dictionary<string, Bloc.BlocType> _blocTypes = InitBlocTypes();

	public static Unit CreateUnit(Unit.ETeam team, string bloc, string source)
	{
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
		unit.BlocType = _blocTypes[bloc];
		unit.SourceType = _sourceTypes[source];

		return unit;
	}

	private static Dictionary<Unit.ETeam, int> InitTeamIds()
	{
		var dict = new Dictionary<Unit.ETeam, int>();

		dict.Add(Unit.ETeam.Monstre, 1);
		dict.Add(Unit.ETeam.Totem, 1);

		return dict;
	}

	private static Dictionary<string, Source.SourceType> InitSourceTypes()
	{
		var dict = new Dictionary<string, Source.SourceType>();

		dict.Add("Foudre", Source.SourceType.Electricity);
		dict.Add("Lave", Source.SourceType.Lava);
		dict.Add("Sable", Source.SourceType.Sand);
		dict.Add("Eau", Source.SourceType.Water);
		dict.Add("Vent", Source.SourceType.Wind);

		return dict;
	}

	private static Dictionary<string, Bloc.BlocType> InitBlocTypes()
	{
		var dict = new Dictionary<string, Bloc.BlocType>();
		
		dict.Add("Terre", Bloc.BlocType.Earth);
		dict.Add("Glace", Bloc.BlocType.Ice);
		dict.Add("Metal", Bloc.BlocType.Metal);
		dict.Add("Plante", Bloc.BlocType.Plant);
		dict.Add("Pierre", Bloc.BlocType.Rock);
		
		return dict;
	}
}
