using UnityEngine;
using System.Collections;

public class ParticleDelete : MonoBehaviour
{
	private AudioSource se;

	// Use this for initialization
	void Start()
	{
		// パーティクルに付属する効果音を取得
		se = this.GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		// 効果音があれば再生終了を待つ
		if (se == null || (se != null && !se.isPlaying))
		{
			if (!GetComponent<ParticleSystem>().IsAlive())
			{
				DestroyObject(this.gameObject);
			}
		}
	}
}
