using UnityEngine;
using System.Collections;
using System;

public class FireNormalPanel : FirePanel
{
	public FireNormalPanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 砲弾を発射します
	/// </summary>
	public override IEnumerator Fire()
	{
		while (tank.hp > 0)
		{
			var heading = tank.lockedObject - tank.transform.position;
			var direction = tank.turretTransform.TransformDirection(Vector3.forward);

			if (Vector3.Angle(heading, direction) < 0.1f)
			{
				tank.Fire();
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public override IEnumerator Lock()
	{
		while (tank.hp > 0)
		{
			// 着弾地点の計算
			float target = (tank.lockedObject - tank.muzzlePosition).magnitude;
			float y = tank.lockedObject.y - tank.muzzlePosition.y + 4;
			float v0 = (tank.firePower * 10000 / Bullet.Mass) * Time.fixedDeltaTime; // 初速度
			float rf = Util.GetAirResistance(Mathf.PI * Mathf.Pow(12f, 2) / 4, Bullet.Mass).magnitude;
			float g = Physics.gravity.magnitude;

			float a = (-(g + rf) * Mathf.Pow(target, 2)) / (2f * Mathf.Pow(v0, 2));
			float b = target / a;
			float c = (a - y) / a;
			float root = Mathf.Pow(-c + Mathf.Pow(b, 2) / 4, 0.5f);
			float ts = Mathf.Pow(b, 2) / 4 - c;
			float angle = 0f;

			if (ts < 0f)
			{
				// 届かない
				tank.Move(1f, 0f);
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
			}
			// 着弾計算終わり

			yield return new WaitForEndOfFrame();
		}
	}
}
