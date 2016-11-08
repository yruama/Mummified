using UnityEngine;
using System.Collections;

public class sound : MonoBehaviour
{
    public AudioClip Sound;

    public void ChangeState()
    {
        if (transform.parent.GetComponent<Menu>().currentMenu == 1)
            GetComponent<AudioSource>().PlayOneShot(Sound);
    }
}
