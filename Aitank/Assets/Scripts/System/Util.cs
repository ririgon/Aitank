using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Util
{
	/// <summary>
	/// コライダーから一番親のゲームオブジェクトを取得します
	/// </summary>
	/// <param name="collider">衝突したコライダー</param>
	/// <returns>衝突したコライダーのルートオブジェクト</returns>
	public static GameObject SearchRootObject(Collider collider)
	{
		Transform root, prev;

		prev = collider.gameObject.transform;
		root = prev.parent;

		while (root != null)
		{
			prev = root;
			root = root.parent;
		}

		return prev.gameObject;
	}

	/*public static Transform GetTankBodyPosition(GameObject root)
	{
		if (root.tag.Equals("Tank"))
		{
			return root.transform.FindChild("Body");
		}

		return null;
	}*/

	/// <summary>
	/// Rect構造体同士の加算を行います
	/// </summary>
	/// <param name="dest"></param>
	/// <param name="src"></param>
	/// <returns></returns>
	public static Rect Add(Rect dest, Rect src)
	{
		return new Rect(dest.x + src.x, dest.y + src.y, dest.width + src.width, dest.height + dest.height);
	}

	/// <summary>
	/// 指定の質量、力、初期位置で時間ごとの物体の位置を取得します
	/// </summary>
	/// <param name="time">時間</param>
	/// <param name="mass">物体の質量</param>
	/// <param name="startPosition">物体の初期位置</param>
	/// <param name="force">物体に与える力</param>
	/// <returns>指定された時間の物体の位置</returns>
	public static Vector3 CalcPositionFromForce(float time, float mass, Vector3 startPosition, Vector3 force)
	{
		float confficient = 4f;
		float airDensity = 1205f;
		float crossSectionalArea = (Mathf.PI * Mathf.Pow(12f, 2)) / 4;
		Vector3 relativeVelocity = new Vector3(0, -0.015f, 0);

		Vector3 speed = (force / mass) * Time.fixedDeltaTime;
		Vector3 resistanceForce = ((airDensity * confficient * crossSectionalArea * relativeVelocity + relativeVelocity) / 2) / mass;
		Vector3 position = (speed * time) + (Physics.gravity + resistanceForce) * (0.5f * Mathf.Pow(time, 2));

		return startPosition + position;
	}

	/// <summary>
	/// 物体に対する空気抵抗力を取得します
	/// </summary>
	/// <param name="crossSectioanlArea">物体の断面積</param>
	/// <param name="mass">物体の質量</param>
	/// <returns>物体に働く空気抵抗のベクトル</returns>
	public static Vector3 GetAirResistance(float crossSectionalArea, float mass)
	{
		float confficient = 4f;
		float airDensity = 1205f;
		Vector3 relativeVelocity = new Vector3(0, -0.015f, 0);

		return ((airDensity * confficient * crossSectionalArea * relativeVelocity + relativeVelocity) / 2) / mass;
	}
}

/// <summary>
/// 空気抵抗に関する値を格納するクラス
/// </summary>
public struct AirResistanceInfo
{
	/// <summary>
	/// 空気抵抗係数
	/// </summary>
	public float coefficient { get; set; }

	/// <summary>
	/// 空気密度
	/// </summary>
	public float airDensity { get; set; }

	/// <summary>
	/// 断面積
	/// </summary>
	public float crossSectionalArea { get; set; }

	/// <summary>
	/// 空気の相対速度
	/// </summary>
	public Vector3 airRelativeVelocity { get; set; }
}