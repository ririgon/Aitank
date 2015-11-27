using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankAI_State : ControllerBase
{
	public SearchPanel searchPanel { set; get; }
	public MovePanel movePanel { set; get; }
	public FirePanel firePanel { set; get; }

	// Use this for initialization
	void Start()
	{
		// 本来画面でカスタマイズする部分
		searchPanel = new SearchNearPanel(tank);
		movePanel = new MoveSinWavePanel(tank);
		firePanel = new FireNormalPanel(tank);
		oneshotFlag = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (!oneshotFlag)
		{
			StartCoroutine(searchPanel.SearchEnemy());
			StartCoroutine(movePanel.Move());
			StartCoroutine(firePanel.Lock());
			StartCoroutine(firePanel.Fire());
			oneshotFlag = true;
		}
	}

	private bool oneshotFlag;
}