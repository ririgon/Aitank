using UnityEngine;
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
}
