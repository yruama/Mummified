using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;

    public GUIText playerHealth;
    public Vector3 offset = new Vector3();

	void Start ()
    {
	}
	
	void Update ()
    {
        playerHealth.text = health + "%";

        playerHealth.transform.position = Camera.main.WorldToViewportPoint(transform.position + offset);
	}
}
