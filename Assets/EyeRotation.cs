using UnityEngine;
using System.Collections;

public class EyeRotation : MonoBehaviour {

	public float speed;
    private float speedInitial;
	private Vector3 _target;
    public float angle;
    public float castTime;

    public float firerate;
    private float nextfire = 0f;

    public Transform player;
    private bool shoot = true;

	void Start ()
	{
        _target = new Vector3 (0, 0, angle);
        speedInitial = speed;

	}
	

	void Update ()
	{                                   //ROTATION
        
        transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, _target, speed * Time.deltaTime);
 
        if (transform.eulerAngles.z == angle)
        {
            _target = Vector3.zero;
        }

        else if (transform.eulerAngles.z == 0)
        {
            _target = new Vector3(0, 0, angle);
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

            Debug.Log(nextfire);
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
