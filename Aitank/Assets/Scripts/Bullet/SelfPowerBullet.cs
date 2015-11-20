using UnityEngine;
using System.Collections;

public class SelfPowerBullet : Bullet
{
	private Vector3 firePower;
	private Vector3 velocity;
	private float time;
	// private Rigidbody body;
	// private Vector3 start;

	// Use this for initialization
	public override void Start()
	{
		// body = GetComponent<Rigidbody>();
		// start = transform.position;
		base.Start();
		
	}

	// Update is called once per frame
	public override void Update()
	{
		Debug.Log(velocity);
		time += Time.deltaTime;

		//transform.position = Util.CalcPositionFromForce(time, body.mass, start, velocity, Physics.gravity, 113f);
		base.Update();
	}

	public void Fire(Vector3 firePower)
	{
		this.velocity = firePower;
	}


}
