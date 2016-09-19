using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour
{
    public Vector3 target;
    public Vector3 offset;

    void OnTriggerEnter2D(Collider2D other)
    {
        float x;
        float y;

        if (other.tag == "Player" || other.tag == "bandage")
        {
            if (target.x == 0)
            {
                x = other.transform.position.x;
            }
            else
            {
                x = target.x;
            }

            if (target.y == 0)
            {
                y = other.transform.position.y;
            }
            else
            {
                y = target.y;
            }

            other.transform.position = new Vector3(x + offset.x, y + offset.y, 0);
        }
    }
}