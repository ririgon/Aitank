using UnityEngine;
using System.Collections;

public abstract class FirePanel : IPanel
{
	public FirePanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 砲弾を発射します
	/// </summary>
	public abstract IEnumerator Fire();

	public abstract IEnumerator Lock();
}
