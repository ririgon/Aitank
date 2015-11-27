using UnityEngine;
using System.Collections;

public abstract class IPanel
{
	public ITank tank { set; get; }

	public IPanel(ITank tank)
	{
		this.tank = tank;
	}
}
