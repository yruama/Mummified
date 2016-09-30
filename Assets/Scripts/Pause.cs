using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    public float timeToHold;
    public GameObject slider;

    public bool pause = false;



	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return) && pause == false)
        {
            GameObject bar = Instantiate(slider) as GameObject;
            bar.transform.SetParent(GameObject.Find("Canvas").transform, false);
            bar.GetComponent<RectTransform>().localPosition = new Vector3(-5, 5, 0);
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            Destroy(GameObject.Find("ProgressRadialHollow(Clone)"));
        }

        if (Input.GetKeyDown(KeyCode.Return) && pause == true)
        {
            Time.timeScale = 1;
            pause = false;
        }
     }
}
