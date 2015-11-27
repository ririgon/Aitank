using UnityEngine;
using System.Collections;
using System;

public class MoveSinWavePanel : MovePanel
{
	public MoveSinWavePanel(ITank tank) : base(tank) { }

	/// <summary>
	/// 戦車を移動させます
	/// </summary>
	public override IEnumerator Move()
	{
		var rigidbody = tank.gameObject.GetComponent<Rigidbody>();

		while (tank.hp > 0)
		{
			if (tank.lockedObject != Vector3.zero)
			{
				var heading = tank.lockedObject - tank.muzzlePosition;
				heading.y = 0;

				var newDir = Vector3.RotateTowards(tank.turretTransform.forward, heading / heading.magnitude, Time.deltaTime * 1, 0f);//heading / heading.magnitude, Time.deltaTime * 1, 0f);
				tank.turretTransform.rotation = Quaternion.LookRotation(newDir);
			}

			

			Vector3 dir = tank.transform.TransformDirection(Vector3.forward);

			rigidbody.AddForce(dir * tank.moveSpeed * 1000f * Mathf.Sin(Time.frameCount * 0.01f));
			yield return new WaitForEndOfFrame();
		}
	}
}
