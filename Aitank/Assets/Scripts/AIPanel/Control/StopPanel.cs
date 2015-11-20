using UnityEngine;
using System.Collections;
using System;

public class StopPanel : ControlPanel
{
	public StopPanel(ITank tank, bool isBlock, params object[] param) : base(tank, isBlock, param)
	{
		
	}

	public override IEnumerator Run()
	{
		throw new NotImplementedException();
	}
}
