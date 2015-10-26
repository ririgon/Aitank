using UnityEngine;
using System.Collections.Generic;

public class TankAI_A : ControllerBase
{

	float time;
	float angle;
	bool rader;
	bool once;

	// Use this for initialization
	void Start()
	{
		rader = false;
		once = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (time > 1f)
		{
			tank.Fire();
			time = 0f;
		}

		if (tank.capturedObject != null && tank.capturedObject.Count > 0)
		{

		}
		else
		{
			if (rader && angle < 150)
			{
				tank.RotateRader(1f);
				angle += 1f;
				once = false;
			}
			else if (!rader && angle > -150)
			{
				tank.RotateRader(-1f);
				angle -= 1;
				once = false;
			}
			else if (!once)
			{
				rader = !rader;
				once = true;
			}
		}

		time += Time.deltaTime;
	}
}
