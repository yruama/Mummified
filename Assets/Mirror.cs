using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {

	private Vector3 positionEnter;
	public float offset;


	void OnTriggerEnter (Collider other)
	{
		positionEnter = other.gameObject.transform.position;

		if (gameObject.tag == "RightBoundary")
		{
			other.gameObject.transform.position = new Vector3 (-positionEnter.x, positionEnter.y, positionEnter.z) + new Vector3 (offset, 0, 0);
		}

		else if (gameObject.tag == "LeftBoundary")
		{
			other.gameObject.transform.position = new Vector3 (-positionEnter.x, positionEnter.y, positionEnter.z) - new Vector3 (offset, 0, 0);
		}

		else if (gameObject.tag == "UpBoundary")
		{
			other.gameObject.transform.position = new Vector3 (positionEnter.x, -positionEnter.y, positionEnter.z) + new Vector3 (0, offset, 0);
		}

		else if (gameObject.tag == "DownBoundary")
		{
			other.gameObject.transform.position = new Vector3 (positionEnter.x, -positionEnter.y, positionEnter.z) - new Vector3 (0, offset, 0);
		}
	}

}
