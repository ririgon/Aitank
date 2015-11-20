using UnityEngine;
using System.Collections;

public class TankAI_Dummy : ControllerBase
{
	private float raderAxis;

	// Use this for initialization
	void Start()
	{
		raderAxis = 1f;
	}

	// Update is called once per frame
	void Update()
	{

		if (tank.capturedObject.Count > 0)
		{
			float distance = float.MaxValue;
			Vector3 v = Vector3.zero;

			foreach (var pair in tank.capturedObject)
			{
				var current = Vector3.Distance(tank.transform.position, pair.Value);

				if (distance > Mathf.Min(distance, current))
				{
					distance = Mathf.Min(distance, current);
					v = pair.Value;
				}
			}

			if (v != Vector3.zero)
			{
				var heading = v - tank.muzzlePosition;
				heading.y = 0;

				var newDir = Vector3.RotateTowards(this.transform.forward, heading / heading.magnitude, Time.deltaTime * 1, 0f);//heading / heading.magnitude, Time.deltaTime * 1, 0f);
				tank.transform.rotation = Quaternion.LookRotation(newDir);

				tank.RotateRaderWithTarget(v);

				if (Vector3.Angle(newDir, heading / heading.magnitude) < 0.2f)
				{
					// 着弾地点の計算

					float target = (v - tank.muzzlePosition).magnitude;
                    float y = v.y - tank.muzzlePosition.y + 4;
					float v0 = (tank.firePower * 10000 / Bullet.Mass) * Time.fixedDeltaTime; // 初速度
					float rf = Util.GetAirResistance(Mathf.PI * Mathf.Pow(12f, 2) / 4, Bullet.Mass).magnitude;
					float g = Physics.gravity.magnitude;

					float a = (-(g + rf) * Mathf.Pow(target, 2)) / (2f * Mathf.Pow(v0, 2));
					float b = target / a;
					float c = (a - y) / a;
					float root = Mathf.Pow(-c + Mathf.Pow(b, 2) / 4, 0.5f);
					float ts = Mathf.Pow(b, 2) / 4 - c;
					float angle;

					if (ts < 0f)
					{
						// 届かない
						tank.Move(1f, 0f);
						return;
					}
					else
					{
						// 届くね
						float angle1 = Mathf.Atan((-b / 2) + root) * Mathf.Rad2Deg;
						float angle2 = Mathf.Atan((-b / 2) + -root) * Mathf.Rad2Deg;

						if (angle1 > 30f)
						{
							if (angle2 > 30f)
							{
								tank.Move(1f, 0f);
								return;
							}
							else
							{
								tank.LiftingGunBarrelWithAngle(angle2);
								angle = angle2;
							}
						}
						else
						{
							tank.LiftingGunBarrelWithAngle(angle1);
							angle = angle1;
						}

						float diff = angle - tank.barrelAngle;
						// Debug.Log("diff" + diff);
						if (diff < 0.5f && diff > -0.5f)
						{
							tank.Fire();
						}
					}
					// 終わり
				}
            }
		}
		else
		{
			tank.RotateRader(raderAxis);
		}
	}
}
