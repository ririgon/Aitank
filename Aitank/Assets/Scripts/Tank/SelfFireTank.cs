using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 実験用タンク（組込みはしないけどテスト用に残す）
/// </summary>
public class SelfFireTank : NormalTank
{
	private float conffcient = 10000;
	private Vector3 ePosition;
	private Quaternion eRotation;
	// private Vector3 endPos;

	// Use this for initialization
	public override void Start()
	{
		base.Start();

		prefabBullet = (GameObject)Resources.Load("Prefabs/Bullet");
		ePosition = this.muzzleTransform.position;
		eRotation = this.muzzleTransform.rotation;
		// endPos = ePosition;
	}

	// Update is called once per frame
	public override void Update()
	{
		if (ePosition != this.muzzleTransform.position || eRotation != this.muzzleTransform.rotation)
		{
			ePosition = this.muzzleTransform.position;
			eRotation = this.muzzleTransform.rotation;
			Vector3 vec = this.muzzleTransform.TransformDirection(Quaternion.Euler(0f, 0f, barrelAngle) * Vector3.forward);
			LineRenderer renderer = GetComponent<LineRenderer>();
			renderer.SetVertexCount(500);

			for (float i = 0f; i < 50f; i += 0.1f)
			{
				Vector3 item = Util.CalcPositionFromForce(i, 20, ePosition, vec * firePower * conffcient);
                renderer.SetPosition((int)(i * 10), item);

				if (item.y <= 0f)
				{
					renderer.SetVertexCount((int)((i + 0.1f) * 10));
					break;
				}


			}
		}
		base.Update();
	}

	public override void Fire()
	{
		if (isReloaded)
		{
			// 弾丸を生成してふっ飛ばします
			var bullet = (GameObject)Instantiate(prefabBullet, this.muzzleTransform.position, this.muzzleTransform.rotation);
			Vector3 vec = bullet.GetComponent<Transform>().TransformDirection(Quaternion.Euler(0f, 0f, barrelAngle) * Vector3.forward);
			//bullet.gameObject.GetComponent<SelfPowerBullet>().Fire(vec * firePower * conffcient);
			bullet.gameObject.GetComponent<Rigidbody>().AddForce(vec * firePower * conffcient);
			bullet.GetComponent<Bullet>().Bind(this);
			GetComponent<Rigidbody>().AddForce(-vec * ((firePower * conffcient) / 600));

			// 発射時の爆発エフェクト
			Instantiate(prefabFire, this.muzzleTransform.position, this.muzzleTransform.rotation);

			// リロードタイム
			isReloaded = false;
		}
	}
}
