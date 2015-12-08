using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MoveJellyFishPanel : MovePanel
{
	public MoveJellyFishPanel(ITank tank) : base(tank) { }

	public override IEnumerator Move()
	{
		var rigidbody = tank.gameObject.GetComponent<Rigidbody>();

		while (tank.hp > 0)
		{
			if (tank.lockedObject != Vector3.zero)
			{				
				var heading = tank.lockedObject - tank.muzzlePosition;
				heading.y = 0;
				//Debug.Log(tank.lockedObject);

				var dest = heading - heading.normalized * 60f;
				tank.turretTransform.rotation = Quaternion.RotateTowards(tank.turretTransform.rotation, Quaternion.LookRotation(heading.normalized), Time.deltaTime * 90f);

				//if (Vector3.Distance(tank.transform.position, tank.lockedObject) > 120f)
				{
					tank.transform.rotation = Quaternion.RotateTowards(tank.transform.rotation, Quaternion.LookRotation(dest.normalized), Time.deltaTime * 40f);
					var force = tank.transform.TransformDirection(Vector3.forward * tank.moveSpeed * 1000f);
					force.y = 0;
                    rigidbody.AddForce(force);
				}
				/*else  if (Vector3.Distance(tank.transform.position, tank.lockedObject) < 80f)
				{
					tank.transform.rotation = Quaternion.RotateTowards(tank.transform.rotation, Quaternion.LookRotation(dest.normalized), Time.deltaTime * 60f);
					var force = tank.transform.TransformDirection(Vector3.back * tank.moveSpeed * 1000f);
					force.y = 0;
					rigidbody.AddForce(force);
					
				}
				else
				{
					tank.transform.rotation = Quaternion.RotateTowards(tank.transform.rotation, Quaternion.LookRotation(tank.transform.eulerAngles + Vector3.up * 90f), Time.deltaTime * 60f);
					var force = tank.transform.TransformDirection(Vector3.forward * tank.moveSpeed * 1000f);
					force.y = 0;
					rigidbody.AddForce(force);
				}*/
			//}

			//rigidbody.AddForce(dir * tank.moveSpeed * 1000f * Mathf.Sin(Time.frameCount * 0.01f));
			//}*/
			}

			yield return new WaitForEndOfFrame();
		}
	}
}
