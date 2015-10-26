using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Util
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
}
