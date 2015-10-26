using UnityEngine;
using System.Collections.Generic;
using System;

public class NormalTank : ITank
{

	// Use this for initialization
	void Start()
	{
		this.turretTransform = gameObject.transform.FindChild("Turret").gameObject.GetComponent<Transform>();
		this.gunBarrelTransform = turretTransform.FindChild("GunBarrel").gameObject.GetComponent<Transform>();
		this.raderTransform = turretTransform.FindChild("Rader").gameObject.GetComponent<Transform>();
		this.ejectionTransform = gunBarrelTransform.FindChild("Ejection").gameObject.GetComponent<Transform>();

		this.raderAngle = this.raderTransform.eulerAngles.y;
		this.turretAngle = this.turretTransform.eulerAngles.y;

		prefabBullet = (GameObject)Resources.Load("Prefabs/Bullet");
		prefabFire = (GameObject)Resources.Load("Prefabs/Fire");
		prefabDestroy = (GameObject)Resources.Load("Prefabs/Destroy");

		capturedObject = new Dictionary<string, Vector3>();

		hp = 30;

		reloadingTime = 0f;
		isReloaded = true;
	}

	// Update is called once per frame
	void Update()
	{
		// リロードタイム
		if (!isReloaded)
		{
			if (reloadingTime > reloadTime)
			{
				isReloaded = true;
				reloadingTime = 0;
			}

			reloadingTime += Time.deltaTime;
		}
	}

	/// <summary>
	/// 車体を移動します
	/// </summary>
	/// <param name="moveAxis"></param>
	/// <param name="rotateAxis"></param>
	public override void Move(float moveAxis, float rotateAxis)
	{
		Vector3 vec = this.GetComponent<Transform>().TransformDirection(Vector3.forward);
		this.GetComponent<Rigidbody>().AddForce(vec * moveAxis * moveSpeed);
		this.GetComponent<Transform>().Rotate(Vector3.up * rotateAxis * 1);
	}

	/// <summary>
	/// 砲弾を発射します
	/// </summary>
	public override void Fire()
	{
		if (isReloaded)
		{
			// 弾丸を生成してふっ飛ばします
			bullet = (GameObject)Instantiate(prefabBullet, this.ejectionTransform.position, this.ejectionTransform.rotation);
			Vector3 vec = bullet.GetComponent<Transform>().TransformDirection(Quaternion.Euler(0f, 0f, gunBarrelAngle) * Vector3.forward);
			bullet.gameObject.GetComponent<Rigidbody>().AddForce(vec * firePower);
			bullet.GetComponent<Bullet>().Bind(this);
			GetComponent<Rigidbody>().AddForce(-vec * (firePower / 600));

			// 発射時の爆発エフェクト
			Instantiate(prefabFire, this.ejectionTransform.position, this.ejectionTransform.rotation);

			// リロードタイム
			isReloaded = false;
		}
	}

	/// <summary>
	/// 砲台を回転します
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public override void RotateTurret(float angleAxis)
	{
		turretAngle += angleAxis;
		turretAngle = ClampAngle(turretAngle);

		// 現在の角度と指定角度の差を求める
		float angleDiff = this.turretTransform.eulerAngles.y + turretAngle;

		// 差分だけ回転させる
		this.turretTransform.Rotate(Vector3.down * angleDiff);
	}

	/// <summary>
	/// 砲身を昇降します
	/// </summary>
	/// <param name="angleAxis">昇降方向(1:上 -1:下)</param>
	public override void LiftingGunBarrel(float angleAxis)
	{
		// 許容角度は0～20°
		if ((gunBarrelAngle < 20f && angleAxis >= 0) || (gunBarrelAngle > 0f && angleAxis <= 0f))
		{
			gunBarrelAngle += angleAxis;
			gunBarrelAngle = ClampAngle(gunBarrelAngle);

			// 現在の角度と指定角度の差を求める
			float angleDiff = this.gunBarrelTransform.eulerAngles.x + gunBarrelAngle;

			// 差分だけ回転させる
			this.gunBarrelTransform.Rotate(Vector3.left * angleDiff);
		}
	}

	/// <summary>
	/// レーダーを回転します
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public override void RotateRader(float angleAxis)
	{
		raderAngle += angleAxis;
		raderAngle = ClampAngle(raderAngle);

		// 現在の角度と指定角度の差を求める
		float angleDiff = this.raderTransform.eulerAngles.y + raderAngle;

		// 差分だけ回転させる
		this.raderTransform.Rotate(Vector3.down * angleDiff);
	}

	/// <summary>
	/// 戦車を破壊します
	/// </summary>
	public override void Destruct()
	{
		Instantiate(prefabDestroy, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}

	public float ClampAngle(float angle)
	{
		if (angle < 0)
			angle += 360;
		if (angle > 360)
			angle -= 360;

		return Mathf.Clamp(angle, 0, 360);
	}

	void OnTriggerEnter(Collider collider)
	{

	}

	void OnTriggerStay(Collider collider)
	{
		GameObject root = Util.SearchRootObject(collider);

		// 捕捉した戦車を記録
		if (root.tag.Equals("Tank") && !collider.name.Equals("Area"))
		{
			Debug.Log("[" + this.gameObject.name + ":Rader] Tank Found: " + root.name);

			if (!capturedObject.ContainsKey(root.name))
				capturedObject.Add(root.name, root.transform.position);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		GameObject root = Util.SearchRootObject(collider);

		// 索敵範囲から外れたら記録を破棄
		if (root.tag.Equals("Tank") && !collider.name.Equals("Area"))
		{
			Debug.Log("[" + this.gameObject.name + ":Rader] Tank Lost: " + root.name);

			if (capturedObject.ContainsKey(root.name))
				capturedObject.Remove(root.name);
		}
	}

	private GameObject prefabBullet;
	private GameObject bullet;
	private GameObject prefabFire;
	private GameObject prefabDestroy;

	private Transform ejectionTransform;
	private Transform gunBarrelTransform;
	private Transform turretTransform;
	private Transform raderTransform;

	private float reloadingTime;
	private bool isReloaded;
}
