using UnityEngine;
using System.Collections;

public class PEye : MonoBehaviour
{
    private PlayerManager player;
    private GameObject arrow;

    public Vector2 angle;
    public float speed;
    public float castTime;

    private float speedInitial;
    private Vector3 _target;

    private float _time;
    public float time;
    private bool _attack;

    public int damage;

    public float firerate;
    private float nextfire = 0f;

   // public Transform player;
    private bool shoot = true;

    void Start()
    {
        player = GetComponent<PlayerManager>();

        
        _attack = false;
        _target = new Vector3(0, 0, angle.y);
        speedInitial = speed;

        SetPosition();

    }


    void Update()
    {
        arrow.transform.eulerAngles = Vector3.MoveTowards(arrow.transform.eulerAngles, _target, speed * Time.deltaTime);

        if (arrow.transform.eulerAngles.z == angle.y)
        {
            _target = new Vector3(0, 0, angle.x);
        }

        else if (arrow.transform.eulerAngles.z == angle.x)
        {
            _target = new Vector3(0, 0, angle.y);
        }

        //TIR

        if ((Input.GetButtonDown("Attack_" + player.playerId) || Input.GetButtonDown("Jump_" + player.playerId)) && shoot == true)
        {
            speed = 0;
            Invoke("Shoot", castTime);
        }

        //RECHARGE

        if (shoot == false /*&& Time.time > nextfire*/)
        {
            shoot = true;
        }

        if (_attack && Time.time - _time > time)
        {
            arrow.transform.GetChild(0).gameObject.SetActive(false);
            _attack = false;
            speed = speedInitial;
        }

    }


    void Shoot()
    {
        _time = Time.time;
        _attack = true;
        arrow.transform.GetChild(0).gameObject.SetActive(true);
        arrow.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = player.color;
        nextfire = Time.time + firerate;
        
        shoot = false;
    }

    void SetPosition()
    {
        int i = Random.Range(0, 4);

        while (checkPosition(i) != true)
        {
            i = Random.Range(0, 4);
        }

        player.gmg.indexEye.Add(i);
        transform.position = player.gmg.eyesPosition[i].position;
        arrow = player.pAttack.arrow;
        
        arrow.transform.SetParent(transform);
        arrow.transform.localPosition = Vector3.zero;
        arrow.transform.eulerAngles = new Vector3(0, 0, angle.x);

        if (i < 2)
        {
            transform.localScale = new Vector3(-2, 2, 0);
        }
    }

    bool checkPosition(int i)
    {
        int j = 0;

        while (j < player.gmg.indexEye.Count)
        {
            if (i == player.gmg.indexEye[j])
                return false;
            j = j + 1;
        }     
        return true;
    }

}