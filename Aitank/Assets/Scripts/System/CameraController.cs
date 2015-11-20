using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
	public int Sensitivity;

	private float x;
	private float y;

	private float yMaxLimit = 80;
	private float yMinLimit = -80;

	private bool ctrlSw;

	/* private GameObject prefabFire; */

	// Use this for initialization
	void Start()
	{
		Vector3 angels = transform.eulerAngles;
		x = angels.y;
		y = angels.x;

		ctrlSw = false;
		/* prefabFire = (GameObject)Resources.Load("Prefabs/Fire"); */

	}

	void Update()
	{
		if (Input.GetButtonDown("Switch"))
		{
			ctrlSw = !ctrlSw;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (ctrlSw)
		{
			Rotate(Input.GetAxis("MouseVertical"), Input.GetAxis("MouseHorizontal"));

			Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
		}

		// マウスクリックした場所にパーティクル生成
		/*
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);

            Instantiate(prefabFire, hit.point, Quaternion.identity);
		}
		*/
	}

	void Move(float x, float z)
	{
		Vector3 vecZ = transform.TransformDirection(Vector3.forward);
		Vector3 vecX = transform.TransformDirection(Vector3.right);
		var position = this.transform.position;

		// 衝突判定
		RaycastHit hit;

		if ((Math.Abs(x) + Math.Abs(z)) != 0f)
		{
			// カメラの移動する方向にレイキャストを飛ばす
			Physics.Raycast(transform.position, (vecZ * x + vecX * z), out hit);

			// 衝突したコライダーがレーダーの索敵範囲だったら無視
			// 衝突先がない場合距離は0になる
			if (hit.distance == 0 || hit.distance > 8f || (hit.collider != null && hit.collider.name.Equals("Area")))
				position += (vecZ * x + vecX * z) * 0.6f;
		}

		if (position.y < 0.5f)
			position.y = 0.5f;

		this.transform.position = position;
	}

	void Rotate(float x, float y)
	{
		this.x += x * Sensitivity * 0.02f;
		this.y -= y * Sensitivity * 0.02f;
		//Debug.Log("x = " + x + ", y = " + y);

		this.y = ClampAngle(this.y, yMinLimit, yMaxLimit);

		var rotation = Quaternion.Euler(this.y, this.x, 0);
		transform.rotation = rotation;
	}

	float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;

		return Mathf.Clamp(angle, min, max);
	}
}
