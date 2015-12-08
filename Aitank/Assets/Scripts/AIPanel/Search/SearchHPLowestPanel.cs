using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SearchHPLowestPanel : SearchPanel
{
	public SearchHPLowestPanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 捕捉した戦車のうち最もHPが少ない戦車の座標を返します
	/// </summary>
	/// <returns>最もHPが少ない戦車の座標</returns>
	public override IEnumerator SearchEnemy()
	{
		while (tank.hp > 0)
		{
			float hp = float.MaxValue;
			Vector3 target = Vector3.zero;

			foreach (var pair in tank.capturedObject)
			{
				if (pair.Value != null)
				{
					if (hp > Mathf.Min(hp, pair.Value.hp))
					{
						hp = Mathf.Min(hp, pair.Value.hp);
						target = pair.Value.transform.position;
					}
				}
			}

			tank.lockedObject = target;

			yield return new WaitForEndOfFrame();
		}
	}
}
