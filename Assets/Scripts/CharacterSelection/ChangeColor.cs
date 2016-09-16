using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

	private int ID = 0;
	private int colorIndex;

	private Color selection;
	ListManager script;

	private bool validation = true;


	void Start ()
    {
		transform.GetComponent<SpriteRenderer>().color = GameObject.Find("ListManager").GetComponent<ListManager>().colorsAvailable[ID];
		colorIndex = ID;

		script = GameObject.Find ("ListManager").GetComponent<ListManager> ();

	}

	void Update ()
    {

			
        																	//CHANGE COLOR   
		if (Input.GetKeyDown(KeyCode.S) && validation == true)
        {
			if (colorIndex == (script.colorsAvailable.Count) - 1)
            {
				colorIndex = 0;
            }

            else
            {
				colorIndex += 1;

            }

			transform.GetComponent<SpriteRenderer>().color = script.colorsAvailable[colorIndex];
        }

		if (Input.GetKeyDown(KeyCode.Q) && validation == true)
        {
			if (colorIndex <= 0)
            {
				colorIndex = (script.colorsAvailable.Count) - 1;
            }

            else
            {
				colorIndex -= 1;
            }

			transform.GetComponent<SpriteRenderer>().color = script.colorsAvailable[colorIndex];
		}

																		// VALIDATION COLOR


		if (Input.GetKeyDown (KeyCode.A))
		{
			selection = script.colorsAvailable[colorIndex];

			script.colorsUnavailable.Add(selection);
			script.colorsAvailable.Remove (selection);

			colorIndex -= 1;
			validation = false;

		}
			

		if (Input.GetKeyDown (KeyCode.Z))
		{
			script.colorsUnavailable.Remove(selection);
			script.colorsAvailable.Add(selection);

			validation = true;
		}



	}
}
