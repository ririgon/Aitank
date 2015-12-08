using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BorderColor : MonoBehaviour
{

	[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
	public Color color;

	[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
	private Color prevColor;
	private Image image;
	private Material mat;

	// Use this for initialization
	void Start()
	{
		this.image = GetComponent<Image>();
		this.mat = new Material(this.image.material.shader);
		this.mat.CopyPropertiesFromMaterial(this.image.material);
		this.mat.SetColor("_Color", color);
		this.image.material = mat;
		this.prevColor = this.color;
	}

	// Update is called once per frame
	void Update()
	{
		if (this.color != this.prevColor)
		{
			this.image.material.SetColor("_Color", color);
			this.prevColor = this.color;
		}
	}
}
