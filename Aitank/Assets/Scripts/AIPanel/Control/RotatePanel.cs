using UnityEngine;
using System.Collections;
using System;

public class RotatePanel : ControlPanel
{
	public RotatePanel(ITank tank, bool isBlock, params object[] param) : base(tank, isBlock, param)
	{

	}

	public override IEnumerator Run()
	{
		isProccessing = true;

		if (tank != null)
		{
			if (param[0].GetType() == typeof(float))
			{
			//	Vector3 direction = (Vector3)param[0];
			//	float distance = (float)param[1];

				tank.Move(0, (float)param[0]);
			}
		}

		isProccessing = false;
		yield return 0;
	}
}
