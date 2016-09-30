using UnityEngine;
using System.Collections;
using ProgressBar;

public class PBBehaviour : MonoBehaviour {

    ProgressRadialBehaviour BarBehaviour;


	void Update ()
    {
        BarBehaviour = GetComponent<ProgressRadialBehaviour>();
        BarBehaviour.Value = 100;
        BarBehaviour.ProgressSpeed = GameObject.Find("PauseManager").GetComponent<Pause>().timeToHold;
	}
}
