using UnityEngine;
using System.Collections;

public class JellyFish : TankAI_State
{

	// Use this for initialization
	public override void Start()
	{
		searchPanel = new SearchHPLowestPanel(tank);
		movePanel = new MoveJellyFishPanel(tank);
		firePanel = new FireNormalPanel(tank);
	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();

		if (tank.transform.position.y > 0.8f)
		{
			var pos = tank.transform.position;
			pos.y = 0.5f;
			tank.transform.position = pos;
			var ro = tank.transform.rotation;
			var angles = ro.eulerAngles;
			angles.x = 0;
			angles.z = 0;
			ro.eulerAngles = angles;
			tank.transform.rotation = ro;
		}
	}
}
