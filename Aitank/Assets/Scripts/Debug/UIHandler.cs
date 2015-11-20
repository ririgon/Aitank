using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour
{

	Dropdown dropdown;
	InputField field;
	GameObject tank;

	// Use this for initialization
	void Start()
	{
		dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
		field = GameObject.Find("InputField").GetComponent<InputField>();
		tank = GameObject.Find("CPU_RED");
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnSubmit()
	{
		var value = System.Single.Parse(field.text);
		Vector3 direction;
		Vector3 dest;

		switch (dropdown.value)
		{
			case 0:
				direction = Vector3.forward;
				break;
			case 1:
				direction = Vector3.back;
				break;
			case 2:
				direction = Vector3.right;
				break;
			case 3:
				direction = Vector3.left;
				break;
			default:
				direction = Vector3.zero;
				break;
		}

		dest = tank.transform.TransformDirection(direction) * value;
		tank.GetComponent<ITank>().Move(dest);
	}

	public void OnCancel()
	{
		tank.GetComponent<ITank>().Stop();
	}
}
