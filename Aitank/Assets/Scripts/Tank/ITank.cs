using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 戦車の基底クラス
/// </summary>
public abstract class ITank : MonoBehaviour
{
	public virtual void Start()
	{
		// 各部位のTransformを取得しておく
		this.turretTransform = gameObject.transform.FindChild("Turret").gameObject.GetComponent<Transform>();
		this.barrelTransform = turretTransform.FindChild("Barrel").gameObject.GetComponent<Transform>();
		this.raderTransform = turretTransform.FindChild("Rader").gameObject.GetComponent<Transform>();
		this.muzzleTransform = barrelTransform.FindChild("Muzzle").gameObject.GetComponent<Transform>();
	}

	/// <summary>
	/// 車体を移動します
	/// </summary>
	/// <param name="moveAxis">直線移動方向</param>
	/// <param name="rotateAxis">回転方向</param>
	public abstract void Move(float moveAxis, float rotateAxis);

	/// <summary>
	/// 方向と移動量を指定して車体を移動します
	/// </summary>
	/// <param name="dest">向きと長さを含んだベクトル</param>
	public abstract void Move(Vector3 vector);

	/// <summary>
	/// 車体を指定の方角に回転します
	/// </summary>
	/// <param name="direction">向きを含むベクトル</param>
	public abstract void Rotate(Vector3 direction);

	/// <summary>
	/// 車体の移動を中断します
	/// </summary>
	public abstract void Stop();

	/// <summary>
	/// 砲弾を発射します
	/// </summary>
	public abstract void Fire();

	/// <summary>
	/// 砲台を回転します
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public abstract void RotateTurret(float angleAxis);

	/// <summary>
	/// 砲台を指定のターゲットに向くように回転させます
	/// </summary>
	/// <param name="target">ターゲット</param>
	public abstract void RotateTurretWithTarget(Vector3 target);

	/// <summary>
	/// 砲身を昇降します
	/// </summary>
	/// <param name="angleAxis">昇降方向(1:上 -1:下)</param>
	public abstract void LiftingGunBarrel(float angleAxis);

	/// <summary>
	/// 砲身を指定角度に昇降します
	/// </summary>
	/// <param name="angle">角度</param>
	public abstract void LiftingGunBarrelWithAngle(float angle);

	/// <summary>
	/// レーダーを回転させます
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public abstract void RotateRader(float angleAxis);

	/// <summary>
	/// レーダーを指定のターゲットに向くように回転させます
	/// </summary>
	/// <param name="target">ターゲット</param>
	public abstract void RotateRaderWithTarget(Vector3 target);

	/// <summary>
	/// 戦車を破壊します
	/// </summary>
	public abstract void Destruct();

	/// <summary>
	/// 戦車のHP
	/// </summary>
	public int hp
	{
		set
		{
			_hp = value;

			// HPが0になったら戦車は破壊されます
			if (_hp <= 0)
			{
				_hp = 0;
				Destruct();
			}
		}

		get
		{
			return _hp;
		}
	}

	/// <summary>
	/// レーダーが捕捉したオブジェクトの座標が格納されます
	/// </summary>
	public Dictionary<string, ITank> capturedObject
	{
		set { this._capturedObject = value; }
		get { return this._capturedObject; }
	}

	public Vector3 lockedObject
	{
		set { this._lockedObject = value; }
		get { return this._lockedObject; }
	}

	/// <summary>
	/// 戦車の砲口の位置
	/// </summary>
	public abstract Vector3 muzzlePosition { get; }

	/// <summary>
	/// 戦車の砲弾を発射する力（弾速・飛距離に関わる）
	/// </summary>
	public float firePower
	{
		set { this._firePower = value; }
		get { return this._firePower; }
	}

	/// <summary>
	/// 砲身の角度
	/// </summary>
	public float barrelAngle
	{
		set { this._barrelAngle = value; }
		get { return this._barrelAngle; }
	}

	/// <summary>
	/// 砲台の角度
	/// </summary>
	public float turretAngle
	{
		set { this._turretAngle = value; }
		get { return this._turretAngle; }
	}

	/// <summary>
	/// レーダーの角度
	/// </summary>
	public float raderAngle
	{
		set { this._raderAngle = value; }
		get { return this._raderAngle; }
	}

	/// <summary>
	/// 移動速度
	/// </summary>
	public float moveSpeed
	{
		set { this._moveSpeed = value; }
		get { return this._moveSpeed; }
	}

	/// <summary>
	/// 砲弾のリロードタイム
	/// </summary>
	public float reloadTime
	{
		set { this._reloadTime = value; }
		get { return this._reloadTime; }
	}

	[HideInInspector]
	public Transform muzzleTransform;
	[HideInInspector]
	public Transform barrelTransform;
	[HideInInspector]
	public Transform turretTransform;
	[HideInInspector]
	public Transform raderTransform;

	#region Private Variables
	[SerializeField, HideInInspector]
	private int _hp;
	[SerializeField, HideInInspector]
	private float _firePower;
	[SerializeField, HideInInspector]
	private float _barrelAngle;
	[SerializeField, HideInInspector]
	private float _turretAngle;
	[SerializeField, HideInInspector]
	private float _raderAngle;
	[SerializeField, HideInInspector]
	private float _moveSpeed;
	[SerializeField, HideInInspector]
	private float _reloadTime;
	[SerializeField, HideInInspector]
	private Dictionary<string, ITank> _capturedObject;
	[SerializeField, HideInInspector]
	private Vector3 _lockedObject;
	#endregion
}
