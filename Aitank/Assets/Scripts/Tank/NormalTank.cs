using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 汎用たんく
/// </summary>
public class NormalTank : ITank
{

	#region Start and Update
	// Use this for initialization
	public override void Start()
	{
		base.Start();

		// それぞれの現在の角度を取得しておく
		this.raderAngle = this.raderTransform.eulerAngles.y;
		this.turretAngle = this.turretTransform.eulerAngles.y;
		this.barrelAngle = this.barrelTransform.eulerAngles.magnitude;

		// 各エフェクトのプレハブを読み込む
		prefabBullet = (GameObject)Resources.Load("Prefabs/Bullet");
		prefabFire = (GameObject)Resources.Load("Prefabs/Fire");
		prefabDestroy = (GameObject)Resources.Load("Prefabs/Destroy");

		capturedObject = new Dictionary<string, ITank>();

		// リロードタイムの初期化
		reloadingTime = 0f;
		isReloaded = true;

		// 最大射程を算出する
		var velocity = (firePower * 10000 / Bullet.Mass) * Time.fixedDeltaTime;
		var maxAngleRad = maxAngle * Mathf.Deg2Rad;
		var rf = Util.GetAirResistance(Mathf.PI * Mathf.Pow(12f, 2) / 4, Bullet.Mass).magnitude;
		var g = Physics.gravity.magnitude;

		Debug.Log(g);
		Debug.Log(rf);
		Debug.Log(velocity);
		maxRange = velocity * Mathf.Cos(maxAngleRad) * (2 * (velocity * Mathf.Sin(maxAngleRad)) / (g + rf));


		// コルーチンフラグ
		liftCo = null;
		rotateRCo = null;
		rotateTCo = null;
	}

	// Update is called once per frame
	public virtual void Update()
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
	#endregion

	#region Moving
	/// <summary>
	/// 車体を移動します
	/// </summary>
	/// <param name="moveAxis"></param>
	/// <param name="rotateAxis"></param>
	public override void Move(float moveAxis, float rotateAxis)
	{
		// 戦車が向いている向きのベクトルを取得
		Vector3 vec = this.GetComponent<Transform>().TransformDirection(Vector3.forward);
		vec.y = 0;	// yは0にしないと戦車が傾いたら空飛んじゃう

		this.GetComponent<Rigidbody>().AddForce(vec * moveAxis * moveSpeed * confficent);
		this.GetComponent<Transform>().Rotate(Vector3.up * rotateAxis * 1);
	}

	/// <summary>
	/// 方向と移動量を指定して車体を移動します
	/// </summary>
	/// <param name="vector">向きと長さを含んだベクトル</param>
	public override void Move(Vector3 vector)
	{
		// ターゲット移動は重複できません
		if (moveCo != null)
		{
			StopCoroutine(moveCo);
			moveCo = null;
		}

		moveCo = StartCoroutine(MoveToDest(vector));
	}


	// 与えられた移動量と向きに従って移動するコルーチン
	System.Collections.IEnumerator MoveToDest(Vector3 vector)
	{

		var rigidbody = this.GetComponent<Rigidbody>();
		var target = this.transform.position + vector;
		var origin = this.transform.position;
		var distance = Vector3.Distance(origin, target);
		// Vector3 newDir = Vector3.RotateTowards(this.transform.forward, vector / vector.magnitude, Time.deltaTime * 1, 0f);

		// 後ろに進む場合
		if (vector.normalized == -this.transform.forward)
		{
			while (Vector3.Distance(this.transform.position, origin) < distance)
			{
				// yベクトルを0にする（じゃないと空飛ぶ戦車が出来上がる）
				var direction = this.transform.forward;
				direction.y = 0;

				rigidbody.AddForce(-direction /*dest.normalized */ * moveSpeed * confficent);

				yield return new WaitForEndOfFrame();
			}
		}
		else
		{
			// 目的地までの距離が誤差の範囲内に縮むまで
			while (Vector3.Distance(this.transform.position, origin) < distance)
			{
				// yベクトルを0にする（じゃないと空飛ぶ戦車が出来上がる）
				var direction = this.transform.forward;
				direction.y = 0;

				// 先に目的地までの角度に回転する
				/*if (Vector3.Angle(newDir, vector / vector.magnitude ) > 0.1f )
				{
					newDir = Vector3.RotateTowards(this.transform.forward, vector / vector.magnitude, Time.deltaTime * 1, 0f);//heading / heading.magnitude, Time.deltaTime * 1, 0f);
					this.transform.rotation = Quaternion.LookRotation(newDir);
				}*/
				//else
				{
					// 回転済んだら前進しましょう
					rigidbody.AddForce(direction * moveSpeed * confficent);
				}

				yield return new WaitForEndOfFrame();
			}
		}

		moveCo = null;
	}

	#endregion

	#region TankRotate
	public override void Rotate(Vector3 direction)
	{
		throw new NotImplementedException();
	}
	#endregion

	/// <summary>
	/// 車体の移動を中断します
	/// </summary>
	public override void Stop()
	{
		if (moveCo != null)
		{
			StopCoroutine(moveCo);
			moveCo = null;
		}
	}

	/// <summary>
	/// 戦車を破壊します
	/// </summary>
	public override void Destruct()
	{
		string name = this.gameObject.name;

		// 爆発エフェクト生成後昇天
		Instantiate(prefabDestroy, transform.position, Quaternion.identity);
		Destroy(this.gameObject);

		// 自身の破壊をマネージャーに通知
		StageManager.Instance.SignalDestruct(name);
	}

	/// <summary>
	/// 砲弾を発射します
	/// </summary>
	public override void Fire()
	{
		if (isReloaded)
		{
			// 弾丸を生成してふっ飛ばします
			var bullet = (GameObject)Instantiate(prefabBullet, this.muzzleTransform.position, this.muzzleTransform.rotation);
			Vector3 vec = bullet.GetComponent<Transform>().TransformDirection(Quaternion.Euler(0f, 0f, barrelAngle) * Vector3.forward);
			bullet.gameObject.GetComponent<Rigidbody>().AddForce(vec * firePower * 10000);
			bullet.GetComponent<Bullet>().Bind(this);
			GetComponent<Rigidbody>().AddForce(-vec * ((firePower * 10000) / 600));

			// 発射時の爆発エフェクト
			Instantiate(prefabFire, this.muzzleTransform.position, this.muzzleTransform.rotation);

			// リロードタイム
			isReloaded = false;
		}
	}

	#region Turret
	/// <summary>
	/// 砲台を回転します
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public override void RotateTurret(float angleAxis)
	{
		turretAngle += angleAxis;

		this.turretTransform.Rotate(Vector3.down * angleAxis);
	}

	/// <summary>
	/// 砲台を指定のターゲットに向くように回転させます
	/// </summary>
	/// <param name="target">ターゲット</param>
	public override void RotateTurretWithTarget(Vector3 target)
	{
		if (rotateTCo != null)
		{
			return;
		}

		rotateTCo = StartCoroutine(RotateTurretCo(target));
	}

	private IEnumerator RotateTurretCo(Vector3 target)
	{
		float time = 0f;
		Vector3 heading = target - this.turretTransform.position;

		// 1秒間の間に指定の向きまでの回転を済ませる(1秒間の間に回転しきれなかったらそれは誤差として・・)
		while (time < 1f)
		{
			Vector3 newDir = Vector3.RotateTowards(this.turretTransform.forward, heading / heading.magnitude, Time.deltaTime, 0f);

			turretTransform.rotation = Quaternion.LookRotation(newDir);
			turretAngle = this.turretTransform.rotation.eulerAngles.y;

			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		rotateTCo = null;
	}
	#endregion

	#region Barrel
	/// <summary>
	/// 砲身を昇降します
	/// </summary>
	/// <param name="angleAxis">昇降方向(1:上 -1:下)</param>
	public override void LiftingGunBarrel(float angleAxis)
	{
		// 許容角度は0～20°
		if ((barrelAngle < 20f && angleAxis >= 0) || (barrelAngle > -8f && angleAxis <= 0f))
		{
			/* 絶対角での指定にしたかった
			gunBarrelAngle += angleAxis;
			gunBarrelAngle = ClampAngle(gunBarrelAngle);

			// 現在の角度と指定角度の差を求める
			float angleDiff = this.gunBarrelTransform.eulerAngles.x + gunBarrelAngle;

			// 差分だけ回転させる
			this.gunBarrelTransform.Rotate(Vector3.left * angleDiff);
			*/
			barrelAngle += angleAxis;

			this.barrelTransform.Rotate(Vector3.left * angleAxis);
		}
	}

	/// <summary>
	/// 砲身を指定角度に昇降します
	/// </summary>
	/// <param name="angle">角度</param>
	public override void LiftingGunBarrelWithAngle(float angle)
	{
		if (liftCo != null)
		{
			return;
		}

		liftCo = StartCoroutine(LiftingGunBarrelCo(angle));
	}

	private IEnumerator LiftingGunBarrelCo(float angle)
	{
		float time = 0f;
		float diff = angle - barrelAngle;

		// Debug.Log("Diff : " + diff);

		while (time < 1f)
		{
			barrelTransform.Rotate(Vector3.left * (diff * Time.deltaTime));
			barrelAngle += diff * Time.deltaTime;
			
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		liftCo = null;
	}
	#endregion

	#region Rader
	/// <summary>
	/// レーダーを回転します
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public override void RotateRader(float angleAxis)
	{
		/* 絶対角での指定にしたかった
		raderAngle += angleAxis;
		raderAngle = ClampAngle(raderAngle);

		// 現在の角度と指定角度の差を求める
		float angleDiff = this.raderTransform.eulerAngles.y + raderAngle;

		// 差分だけ回転させる
		this.raderTransform.Rotate(Vector3.down * angleDiff);
		*/
		//raderAngle += angleAxis;
		//raderAngle = ClampAngle(raderAngle);
		raderAngle = this.raderTransform.rotation.eulerAngles.y;

		this.raderTransform.Rotate(Vector3.up * angleAxis);
	}

	public override void RotateRaderWithTarget(Vector3 target)
	{
		if (rotateRCo != null)
		{
			return;
		}

		rotateRCo = StartCoroutine(RotateRaderCo(target));
	}

	private IEnumerator RotateRaderCo(Vector3 target)
	{
		float time = 0f;
		Vector3 heading = target - this.raderTransform.position;

		// 1秒間の間に指定の向きまでの回転を済ませる(1秒間の間に回転しきれなかったらそれは誤差として・・)
		while (time < 1f)
		{
			Vector3 newDir = Vector3.RotateTowards(this.raderTransform.forward, heading / heading.magnitude, Time.deltaTime, 0f);

			raderTransform.rotation = Quaternion.LookRotation(newDir);
			raderAngle = this.raderTransform.rotation.eulerAngles.y;

			//Debug.Log("Rader Angle:"+raderAngle+"   Angle:"+ Quaternion.LookRotation(newDir).eulerAngles + "   Diff:"+diff);

			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		rotateRCo = null;
	}

	/// <summary>
	/// 角度を0～360度の間に整形します
	/// </summary>
	/// <param name="angle"></param>
	/// <returns></returns>
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
			//Debug.Log("[" + this.gameObject.name + ":Rader] Tank Found: " + root.name);

			if (!capturedObject.ContainsKey(root.name))
			{
				capturedObject.Add(root.name, root.GetComponent<ITank>());
				//Debug.Log("First find. :" + root.transform.FindChild("Turret").position);
			}
			else
			{
				capturedObject[root.name] = root.GetComponent<ITank>();
				//Debug.Log("Update find. :" + root.transform.FindChild("Turret").position);
			}
		}
	}

	void OnTriggerExit(Collider collider)
	{
		GameObject root = Util.SearchRootObject(collider);

		// 索敵範囲から外れたら記録を破棄
		if (root.tag.Equals("Tank") && !collider.name.Equals("Area"))
		{
			//Debug.Log("[" + this.gameObject.name + ":Rader] Tank Lost: " + root.name);

			if (capturedObject.ContainsKey(root.name))
				capturedObject.Remove(root.name);
		}
	}

	#endregion

	#region Util
	public override Vector3 muzzlePosition
	{
		get
		{
			return this.muzzleTransform.position;
		}
	}
	#endregion

	#region Variables
	public float maxRange;
	public float minRange;

	private float confficent = 1000f;
	private float maxAngle = 30;

	protected GameObject prefabBullet;
	protected GameObject prefabFire;
	private GameObject prefabDestroy;

	private Coroutine moveCo;
	private Coroutine liftCo;
	private Coroutine rotateRCo;
	private Coroutine rotateTCo;

	private float reloadingTime;
	public bool isReloaded { set; get; }
	#endregion
}
