using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchNearPanel : SearchPanel
{
	public SearchNearPanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 捕捉した戦車のうち最も距離が近い戦車の座標を返します
	/// </summary>
	/// <returns>最も距離が近い戦車の座標</returns>
	public override IEnumerator SearchEnemy()
	{
		while (tank.hp > 0)
		{
			float distance = float.MaxValue;
			Vector3 target = Vector3.zero;

			foreach (var pair in tank.capturedObject)
			{
				if (pair.Value != null)
				{
					var current = Vector3.Distance(tank.transform.position, pair.Value.transform.position);

					if (distance > Mathf.Min(distance, current))
					{
						distance = Mathf.Min(distance, current);
						target = pair.Value.transform.position;
					}
				}

			}

			tank.lockedObject = target;

			yield return new WaitForEndOfFrame();
		}
	}
}
