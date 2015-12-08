using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
	public GameObject[] cameras;
	private int number;

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].GetComponent<Camera>().enabled = false;
			cameras[i].GetComponent<AudioListener>().enabled = false;
		}

		number = 0;
		cameras[0].GetComponent<Camera>().enabled = true;
		cameras[0].GetComponent<AudioListener>().enabled = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("CameraSwitchRight"))
		{
			if (cameras[number] != null)
			{
				cameras[number].GetComponent<Camera>().enabled = false;
				cameras[number].GetComponent<AudioListener>().enabled = false;
			}

			do
			{
				number++;

				if (number >= cameras.Length)
				{
					number = 0;
					break;
				}
			} while (number != 0 && cameras[number] == null);

			cameras[number].GetComponent<Camera>().enabled = true;
			cameras[number].GetComponent<AudioListener>().enabled = true;
		}

		if (Input.GetButtonDown("CameraSwitchLeft"))
		{
			cameras[number].GetComponent<Camera>().enabled = false;
			cameras[number].GetComponent<AudioListener>().enabled = false;

			do
			{
				number--;

				if (number < 0)
					number = cameras.Length - 1;
			} while (number > 0 && cameras[number] == null);

			cameras[number].GetComponent<Camera>().enabled = true;
			cameras[number].GetComponent<AudioListener>().enabled = true;
		}
	}
}
