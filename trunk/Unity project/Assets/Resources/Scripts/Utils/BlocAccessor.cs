using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BlocAccessor {

	public Color HighlightColor;

	private Dictionary<Bloc, Bloc> _paths = new Dictionary<Bloc, Bloc>();
	private IEnumerable<Bloc> _accessibleBlocs;

	public IEnumerable<Bloc> AccessibleBlocs
	{
		get { return _accessibleBlocs; }
		private set
		{
			if (_accessibleBlocs != null)
			{
				foreach (Bloc bloc in _accessibleBlocs)
					bloc.RemoveHighlight();
			}
			
			_accessibleBlocs = value;
			
			if (_accessibleBlocs != null)
			{
				foreach (Bloc bloc in _accessibleBlocs)
					bloc.Highlight(HighlightColor);
			}
		}
	}

	public BlocAccessor(Color color)
	{
		HighlightColor = color;
	}

	public void Update(Bloc start, int range)
	{
		AccessibleBlocs = GetAccessibleBlocs(start, range);
	}

	public void Clear()
	{
		AccessibleBlocs = null;
	}

	public List<Bloc> GetPath(Bloc from, Bloc to)
	{
		List<Bloc> path = new List<Bloc>();
		Bloc bloc = to;
		while (bloc != from)
		{
			path.Insert(0, bloc);
			bloc = _paths[bloc];
		}

		return path;
	}

	private IEnumerable<Bloc> GetAccessibleBlocs(Bloc start, int distance)
	{
		if (distance <= 0)
			return null;
		
		List<Bloc> blocs = Map.FetchNeighbors2D(start, 1);
		List<Bloc> neighbours = new List<Bloc>();
		for (int i = 0; i < blocs.Count; ++i)
		{
			if (Math.Abs(blocs[i].indexInMap.z - start.indexInMap.z) > 1
			    || !blocs[i].IsReachable())
			{
				blocs.RemoveAt(i);
				i = i - 1; // compensate for the deletion of an entry
				continue;
			}
			
			_paths[blocs[i]] = start;
			
			IEnumerable<Bloc> directNeighbours = GetAccessibleBlocs(blocs[i], distance - 1);
			if (directNeighbours != null)
				neighbours.AddRange(directNeighbours);
		}
		
		neighbours.ForEach(delegate (Bloc bloc) {
			if (!blocs.Contains(bloc))
				blocs.Add(bloc);
		});
		
		return new HashSet<Bloc>(blocs);
	}
}
