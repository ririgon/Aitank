using UnityEngine;
using System.Collections;
using System;

public class MovePanel : ControlPanel
{
	public MovePanel(ITank tank, bool isBlock, params object[] param) : base(tank, isBlock, param)
	{

	}

	public override IEnumerator Run()
	{
		isProccessing = true;

		if (tank != null)
		{
			if (param[0].GetType() == typeof(Vector3) && param[1].GetType() == typeof(float))
			{
				Vector3 direction = (Vector3)param[0];
				float distance = (float)param[1];

				tank.Move(direction * distance);
			}
		}

		isProccessing = false;
		yield return 0;
	}
}
