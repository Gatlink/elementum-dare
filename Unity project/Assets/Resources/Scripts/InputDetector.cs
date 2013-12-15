using UnityEngine;
using System.Collections;

class InputDetector : MonoBehaviour
{
	void Start() {}

	void Update() 
	{
		if(!Input.anyKeyDown)
			return;

		if(Input.GetKeyDown(KeyCode.Space))
			SourceManager.Instance().SpawnSource(Source.GetRandomSourceType());

		if(Input.GetKeyDown(KeyCode.Return))
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
		}
	}
}