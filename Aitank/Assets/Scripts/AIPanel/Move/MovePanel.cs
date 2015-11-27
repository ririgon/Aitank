using UnityEngine;
using System.Collections;
using System;

public abstract class MovePanel : IPanel
{
	public MovePanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 戦車を移動させます
	/// </summary>
	public abstract IEnumerator Move();
}
