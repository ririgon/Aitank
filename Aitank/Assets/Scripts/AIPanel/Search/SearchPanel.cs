using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SearchPanel : IPanel
{
	public SearchPanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 捕捉した戦車のうち最も距離が近い戦車の座標を返します
	/// </summary>
	/// <returns>最も距離が近い戦車の座標</returns>
	public abstract IEnumerator SearchEnemy();
}
