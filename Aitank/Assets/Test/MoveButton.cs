using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveButton : MonoBehaviour
{
	bool isPressed;
	Vector2 defaultLocalPosition;
	Vector2 defaultPosition;
	float r;

	// Use this for initialization
	void Start()
	{
		this.isPressed = false;
		this.defaultLocalPosition = transform.localPosition;
		this.defaultPosition = transform.position;
		r = (GameObject.Find("MoveCtrl_BG").GetComponent<Image>().rectTransform.rect.width / 2) * 0.68f;
	}

	// Update is called once per frame
	void Update()
	{
		if (isPressed)
		{
			Vector2 v = (Vector2)Input.mousePosition - this.defaultPosition;
			Vector2 heading = v - this.defaultLocalPosition;
			float rad = Mathf.Atan2(heading.y, heading.x);

			if (r < heading.magnitude)
			{
				v = (Vector2.right * (r * Mathf.Cos(rad))) + (Vector2.up * (r * Mathf.Sin(rad)));
			}

			this.transform.localPosition = v;
		}
	}

	public void OnPointerDown()
	{
		this.isPressed = true;
	}

	public void OnPointerUp()
	{
		this.isPressed = false;
		this.transform.localPosition = this.defaultLocalPosition;
	}
}
