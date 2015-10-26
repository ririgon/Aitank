using UnityEngine;
using System.Collections;
using System;

public class AirResistance : MonoBehaviour
{
	public float coefficient;   // 空気抵抗係数
	public float airDensity;    // 空気密度
	public float crossSectionalArea;    // 断面積
	public Vector3 airRelativeVelocity; // 空気の相対速度

	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		// 空気抵抗を与える
		rigidbody.AddForce((airDensity * coefficient * crossSectionalArea * airRelativeVelocity + airRelativeVelocity) / 2);
	}

	private new Rigidbody rigidbody;
}
