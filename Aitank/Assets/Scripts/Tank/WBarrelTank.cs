using UnityEngine;
using System.Collections;

public class WBarrelTank : NormalTank
{

	// Use this for initialization
	public override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();
	}

	public override void Fire()
	{
		if (isReloaded)
		{
			// 弾丸を生成してふっ飛ばします

			for (int i = 0; i < this.muzzlePosition.Length; i++)
			{
				var bullet = (GameObject)Instantiate(prefabBullet, this.muzzlePosition[i], this.barrelTransform.rotation);
                Vector3 vec = bullet.GetComponent<Transform>().TransformDirection(Quaternion.Euler(0f, 0f, barrelAngle) * Vector3.forward);
				bullet.gameObject.GetComponent<Rigidbody>().AddForce(vec * firePower * 10000);
				bullet.GetComponent<Bullet>().Bind(this);
				GetComponent<Rigidbody>().AddForce(-vec * ((firePower * 10000) / 600));

				// 発射時の爆発エフェクト
				Instantiate(prefabFire, this.muzzlePosition[i], this.barrelTransform.rotation);
			}

			// リロードタイム
			isReloaded = false;
		}
	}

	#region Util
	public new Vector3[] muzzlePosition
	{
		get
		{
			return new[] { barrelTransform.FindChild("Muzzle").gameObject.GetComponent<Transform>().position,
		barrelTransform.FindChild("MuzzleB").gameObject.GetComponent<Transform>().position };
		}
	}
	#endregion

	#region Variables
	#endregion
}
