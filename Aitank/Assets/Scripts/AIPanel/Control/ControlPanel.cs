using UnityEngine;
using System.Collections;

public abstract class ControlPanel : IPanel
{
	// 操作対象
	protected ITank tank;

	// 引数
	protected object[] param;

	// 実行モード （true: 通過 false:実行が終了するまで待機）
	protected bool isBlock;

	public abstract void Run();
}
