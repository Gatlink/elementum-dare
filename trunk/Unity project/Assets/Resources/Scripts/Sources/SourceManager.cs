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
	
	//Singleton
	private static SourceManager _instance = new SourceManager();
	
	public static SourceManager Instance()
	{
		return _instance;
	}
	
	private SourceManager(){}
}
