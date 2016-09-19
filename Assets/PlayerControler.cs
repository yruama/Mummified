using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

	Rigidbody rb;
	public float speed;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()

	{
		float move_horiz = Input.GetAxis ("Horizontal");
		float move_vert = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (move_horiz, move_vert, 0.0f);

		rb.velocity = movement * speed;

	}
}
