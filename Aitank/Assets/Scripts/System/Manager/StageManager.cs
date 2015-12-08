using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
	private Dictionary<string, GameObject> tankList;

	// Use this for initialization
	void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		tankList = new Dictionary<string, GameObject>();
		GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");

		for (int i = 0; i < tanks.Length; i++)
		{
			tankList.Add(tanks[i].name, tanks[i]);
		}
	}

	public void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			GetComponent<CameraSwitcher>().cameras[0].GetComponent<Camera>().enabled = true;
			GetComponent<CameraSwitcher>().cameras[0].GetComponent<AudioListener>().enabled = true;

			foreach (var tank in tankList)
			{
				tank.Value.GetComponent<ITank>().hp = 0;
			}
		}
	}

	/// <summary>
	/// 戦車の破壊をマネージャーに通知します
	/// </summary>
	/// <param name="objectName">破壊された戦車</param>
	public void SignalDestruct(string objectName)
	{
		StartCoroutine(SignalDestructCo(objectName));
	}

	public IEnumerator SignalDestructCo(string objectName)
	{
		yield return new WaitForEndOfFrame();

		if (tankList.ContainsKey(objectName))
		{
			tankList.Remove(objectName);
			GameObject obj;
			Vector3 pos;
			int num = int.Parse("" + objectName[4]);

			switch(num)
			{
				case 1:
					pos = GameObject.Find("1PSpawner").transform.position;
					obj = Resources.Load("Prefabs/Be-Fes/Tank1") as GameObject;
					break;
				case 2:
					pos = GameObject.Find("2PSpawner").transform.position;
					obj = Resources.Load("Prefabs/Be-Fes/Tank2") as GameObject;
					break;
				case 3:
					pos = GameObject.Find("3PSpawner").transform.position;
					obj = Resources.Load("Prefabs/Be-Fes/Tank3") as GameObject;
					break;
				case 4:
					pos = GameObject.Find("4PSpawner").transform.position;
					obj = Resources.Load("Prefabs/Be-Fes/Tank4") as GameObject;
					break;
				default:
					pos = GameObject.Find(objectName).transform.position;
					obj = Resources.Load("Prefabs/Be-Fes/Tank1") as GameObject;
					break;
			}

			StartCoroutine(DelaySpawn(objectName, obj, pos));

			foreach (var t in tankList)
			{
				if (t.Value != null)
				{
					var capturedObject = t.Value.GetComponent<ITank>().capturedObject;

					if (capturedObject.ContainsKey(objectName))
					{
						capturedObject.Remove(objectName);
						//tankList[t.Key].capturedObject = capturedObject;
					}
				}
			}
		}

		Debug.Log(objectName + " has destroyed.");
	}

	public IEnumerator DelaySpawn(string name, GameObject obj, Vector3 pos)
	{
		yield return new WaitForSeconds(2.5f);
		GameObject newObj = Instantiate(obj, pos, Quaternion.identity) as GameObject;
		newObj.name = name;
		int num = int.Parse("" + name[4]);
		GetComponent<CameraSwitcher>().cameras[num] = newObj.GetComponentInChildren<Camera>().gameObject;

		tankList.Add(newObj.name, newObj);
	}
}
