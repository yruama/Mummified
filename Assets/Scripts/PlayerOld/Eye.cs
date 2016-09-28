using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour
{
    public float speed;
    private float speedInitial;
    private Vector3 _target;
    public Vector2 angle;
    public float castTime;

    public float firerate;
    private float nextfire = 0f;

    public Transform player;
    private bool shoot = true;

    void Start()
    {
        _target = new Vector3(0, 0, angle.y);
        speedInitial = speed;

    }

    void Update()
    {                                   
        //ROTATION
        transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, _target, speed * Time.deltaTime);

        if (transform.eulerAngles.z == angle.y)
        {
            _target = new Vector3(0, 0, angle.x);
        }

        else if (transform.eulerAngles.z == angle.x)
        {
            _target = new Vector3(0, 0, angle.y);
        }

        //TIR
        if (Input.GetKeyDown(KeyCode.Space) && shoot == true)
        {
            speed = 0;
            Invoke("Shoot", castTime);
        }

        //RECHARGE
        if (shoot == false && Time.time > nextfire)
        {
            transform.position = player.transform.position;
            shoot = true;
        }

    }


    void Shoot()
    {
        //Instantier le tir
        nextfire = Time.time + firerate;

        speed = speedInitial;
        transform.position = new Vector3(200, 200, 200); //J'ai fait ça au lieu de destroy la flèche du coup ça la met juste super loin.
        shoot = false;
    }
}