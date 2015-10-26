using UnityEngine;
using System.Collections;

public class TankController : ControllerBase
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		tank.Move(Input.GetAxis("TankVertical"), Input.GetAxis("TankHorizontal"));
		tank.RotateTurret(Input.GetAxis("Turret"));
		tank.LiftingGunBarrel(Input.GetAxis("GunBarrel"));
		tank.RotateRader(1f);

		if (Input.GetButtonDown("Fire"))
		{
			tank.Fire();
		}
	}
}
