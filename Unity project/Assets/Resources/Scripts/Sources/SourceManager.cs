using UnityEngine;
using System.Collections.Generic;

public class SourceManager : IManager<Source>
{
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
	
	public void CleanDeadSources()
	{
		List<Source> markedForDeletion = new List<Source>();
		foreach(Source source in _items)
		{
			if(source.UpdateSourceState())
				markedForDeletion.Add(source);
		}

		if(markedForDeletion.Count > 0)
		{
			foreach(Source corpse in markedForDeletion)
			{
				corpse.RemoveSelf();
				UnregisterElement(corpse);
			}
		}
	}
	
	//Singleton
	private static SourceManager _instance = new SourceManager();
	
	public static SourceManager Instance()
	{
		return _instance;
	}
	
	private SourceManager()	{}
}
