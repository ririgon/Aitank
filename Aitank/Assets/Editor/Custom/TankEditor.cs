using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;

[CustomEditor(typeof(ITank), true)]
public class TankEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var tank = this.target as ITank;

		int tmp_hp = EditorGUILayout.IntField("HP", tank.hp);
		
		if (tmp_hp > 0)
		{
			tank.hp = tmp_hp;
		}
		else if (!EditorApplication.isPlaying && tank.hp <= 0)
		{
			tank.hp = 1;
		}

		tank.firePower = EditorGUILayout.FloatField("Fire Power", tank.firePower);
		tank.moveSpeed = EditorGUILayout.FloatField("Move Speed", tank.moveSpeed);
		tank.reloadTime = EditorGUILayout.FloatField("Reload Time", tank.reloadTime);

		if (tank.capturedObject != null)
		{
			var keys = tank.capturedObject.Keys;
			string[] keyname = keys.ToArray();

			for (int i = 0; i < keyname.Length; i++)
			{
				EditorGUILayout.Vector3Field(keyname[i], tank.capturedObject[keyname[i]].transform.position);
			}
		}
	}
}
