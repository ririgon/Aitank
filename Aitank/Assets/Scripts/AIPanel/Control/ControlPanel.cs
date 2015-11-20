using UnityEngine;
using System.Collections;
using System;

public abstract class ControlPanel : IPanel
{
	// 操作対象
	protected ITank tank;

	// 引数
	protected object[] param;

	// 実行モード （true: 通過 false:実行が終了するまで待機）
	public bool isBlock { set; get; }

	public bool isProccessing { set; get; }

	public ControlPanel(ITank tank, bool isBlock, params object[] param)
	{
		this.tank = tank;
		this.param = param;
		this.isBlock = isBlock;
	}

	public abstract IEnumerator Run();
}
