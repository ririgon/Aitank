using UnityEngine;
using System.Collections.Generic;

public abstract class ITank : MonoBehaviour
{
	public float firePower;
	public float gunBarrelAngle;
	public float turretAngle;
	public float raderAngle;
	public float moveSpeed;
	public float reloadTime;

	/// <summary>
	/// 戦車のHP
	/// </summary>
	public int hp
	{
		get
		{
			return _hp;
		}

		set
		{
			_hp = value;

			if (_hp <= 0)
			{
				_hp = 0;
				Destruct();
			}
		}
	}

	public Dictionary<string, Vector3> capturedObject
	{
		set
		{
			this._capturedObject = value;
		}

		get
		{
			return this._capturedObject;
		}
	}

	/// <summary>
	/// 車体を移動します
	/// </summary>
	/// <param name="moveAxis">直線移動方向</param>
	/// <param name="rotateAxis">回転方向</param>
	public abstract void Move(float moveAxis, float rotateAxis);

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
	/// 砲身を昇降します
	/// </summary>
	/// <param name="angleAxis">昇降方向(1:上 -1:下)</param>
	public abstract void LiftingGunBarrel(float angleAxis);

	/// <summary>
	/// レーダーを回転させます
	/// </summary>
	/// <param name="angleAxis">回転方向 (1:左 -1:右)</param>
	public abstract void RotateRader(float angleAxis);

	/// <summary>
	/// 戦車を破壊します
	/// </summary>
	public abstract void Destruct();

	private int _hp;
	private Dictionary<string, Vector3> _capturedObject;
}
