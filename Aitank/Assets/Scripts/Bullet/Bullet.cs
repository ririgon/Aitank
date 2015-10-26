using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	// 爆発用エフェクト
	private GameObject explosion;

	// この砲弾を発射する戦車
	private ITank bindedTank;

	// Use this for initialization
	void Start()
	{
		this.explosion = (GameObject)Resources.Load("Prefabs/Explosion");
	}

	// Update is called once per frame
	void Update()
	{
		if (this.GetComponent<Transform>().position.y < 0)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		// デバッグ用に着弾地点と発射した戦車との距離を表示
		if (bindedTank != null)
		{
			Debug.Log("[" + bindedTank.name + ":Bullet] Point of impact: " + transform.position);
			Debug.Log("[" + bindedTank.name + ":Bullet] Distance: " + Vector3.Distance(transform.position, bindedTank.transform.position));
		}

		// 他戦車のレーダーのコライダじゃなければ
		if (!collider.name.Equals("Area"))
		{
			GameObject root = Util.SearchRootObject(collider);

			if (root.tag.Equals("Tank"))
			{
				Debug.Log("[" + bindedTank.name + ":Bullet] Hit: " + root.name);
				root.GetComponent<ITank>().hp -= 2;
			}

			// ボカン
			Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
	}

	public void Bind(ITank tank)
	{
		bindedTank = tank;
	}
}
