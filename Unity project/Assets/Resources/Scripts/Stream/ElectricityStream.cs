using UnityEngine;
using System.Collections;

public class ElectricityStream : Stream
{
	private float _animTimer = -1.0f;
	private const float ANIM_TIME = 1.0f;

	public override void UpdateStream()
	{ /*do nothing, handled by the source */}

	public override void UpdateStreamVisual()
	{
		Vector3 bias = new Vector3(1.12f, 1.12f, 1.12f);

		float height = 0;
		if(_bloc.IsFlooded)
		{
			height = _bloc.GetStreamOfType(Source.SourceType.Water).gameObject.transform.localScale.y;
		}

		//Update stream to surround the bloc
		Vector3 blocScale = _bloc.transform.localScale;
		Vector3 streamScale = new Vector3(blocScale.x * bias.x, (blocScale.y * bias.y) + height * 0.5f, blocScale.z * bias.z);
		gameObject.transform.localScale = streamScale;//(_bloc.GetBlocSize() + bias);
		gameObject.transform.position = _bloc.transform.position;
		gameObject.transform.Translate( new Vector3(0.0f, height * 0.5f, 0.0f) );
	}

	public override void Update()
	{
		if(_animTimer <= 0.0f)
		{
			/*_animTimer = ANIM_TIME;
			int comp = Mathf.RoundToInt(Random.value);
			Vector2 offset = gameObject.renderer.material.GetTextureOffset("_MainTex");
			offset[comp] += 90.0f;

			gameObject.renderer.material.SetTextureOffset("_MainTex", offset);*/
		}
		else
			_animTimer -= Time.deltaTime;
	}
}
