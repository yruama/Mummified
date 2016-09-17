using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerMenu : MonoBehaviour
{
    public Sprite[] cadre;
    public Color[] colors;

    [HideInInspector]
    public int[] availableColors;

	void Start ()
    {
        availableColors = new int[colors.Length];
	}
	
	void Update ()
    {
	   
	}
}
