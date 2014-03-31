using UnityEngine;
using System.Collections.Generic;

public class SourceManager : IManager<Source>
{
	private List<Source> _markedForDeletion = new List<Source>();

	public Source SpawnSource(Source.SourceType type)
	{
		Source source = SourceFactory.CreateSource(type);
		RegisterElement(source);
		return source;
	}

	public void UpdateSources()
	{
		foreach(Source source in _items)
		{
			source.RunSource();
		}
	}

	public void FlagDeadSources()
	{
		foreach(Source source in _items)
		{
			if(source.UpdateSourceState())
				_markedForDeletion.Add(source);
		}
	}

	public void RemoveDeadSources()
	{
		foreach(Source corpse in _markedForDeletion)
		{
			corpse.Die();
			UnregisterElement(corpse);
		}
	}

	public List<Bloc> GetSourceBlocs()
	{
		List<Bloc> blocs = new List<Bloc>();
		foreach(Source source in _items)
		{
			blocs.Add(source.Bloc);
		}
		return blocs;
	}

	//Singleton
	private static SourceManager _instance = null;
	public static SourceManager Instance
	{
		get
		{ return (_instance != null) ? _instance : _instance = new SourceManager(); }
	}
	
	private SourceManager(){}
}
