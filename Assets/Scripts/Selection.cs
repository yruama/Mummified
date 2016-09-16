using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selection : MonoBehaviour 
{
	public Color[] colors;
	private int[] dispo;

	private int ID = 0;
	private int colorIndex;
	private int lastColorIndex;

	void Start()
	{
		colorIndex = ID;
		lastColorIndex = colorIndex;
		dispo = new int[colors.Length];
		dispo [colorIndex] = 0; // 0 = indispo 1 = dispo
		dispo[1] = 0;

		Debug.Log (colorIndex);
	}



	void Update()
	{ 
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			while (dispo [colorIndex] != 1) 
			{
				colorIndex += 1;
			}
			dispo [colorIndex] = 0;
			dispo [lastColorIndex] = 1;
			lastColorIndex = colorIndex;
		}

		Debug.Log (colorIndex);
	}
}
