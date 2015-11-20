using UnityEngine;
using System.Collections;

public interface IPanel
{
	// 実行モード （true: 通過 false:実行が終了するまで待機）
	bool isBlock { set; get; }

	bool isProccessing { set; get; }

	IEnumerator Run();
}
