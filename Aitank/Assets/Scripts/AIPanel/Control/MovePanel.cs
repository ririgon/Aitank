using UnityEngine;
using System.Collections;
using System;

public class MovePanel : ControlPanel
{
	public override void Run()
	{
		if (tank != null)
		{
			if (param[0].GetType() == typeof(Vector3) && param[1].GetType() == typeof(float))
			{
				Vector3 direction = (Vector3)param[0];
				float distance = (float)param[1];
				Vector3 origin = tank.transform.position;
				

				while (Vector3.Distance(origin, tank.transform.position) < distance)
				{
				}
			}
		}
	}
}
