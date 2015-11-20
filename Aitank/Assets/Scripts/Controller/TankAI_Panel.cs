using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankAI_Panel : ControllerBase
{
	List<IPanel> panels;
	List<IPanel>.Enumerator it;
	Coroutine ip;
	Coroutine cpu;
	
		
	// Use this for initialization
	void Start()
	{
		panels = new List<IPanel>();
		panels.Add(new MovePanel(tank, false, tank.transform.TransformDirection(Vector3.forward), 10f));
		panels.Add(new RotatePanel(tank, false, 15f));
		it = panels.GetEnumerator();
		cpu = null;
		//tank.Move(tank.transform.forward * 10);
	}

	// Update is called once per frame
	void Update()
	{
		if (cpu == null)
			cpu = StartCoroutine(CPU());
	}

	IEnumerator CPU()
	{
		// 実行されている命令がない
		if (ip == null)
		{
			// 次がないな？
			if (!it.MoveNext())
			{
				// スタートに戻る
				it = panels.GetEnumerator();
			}

			ip = StartCoroutine(it.Current.Run());
		}
		// 現在実行されている命令は非ブロッキング
		else if (!it.Current.isBlock)
		{
			StopCoroutine(ip);

			if (!it.MoveNext())
			{
				it = panels.GetEnumerator();
				it.MoveNext();
			}
			//else
			{
				ip = StartCoroutine(it.Current.Run());
			}
		}
		// ブロッキング命令が終了した
		else if (!it.Current.isProccessing)
		{
			ip = null;
		}

		yield return new WaitForSeconds(0.1f);

		cpu = null;
	}
}
