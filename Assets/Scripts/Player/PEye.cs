using UnityEngine;
using System.Collections;

public class PEye : MonoBehaviour
{
    private PlayerManager player;
    private GameObject arrow;
    private Color _c;
    public Vector2 angle;
    public float speed;
    public float castTime;

    private float speedInitial;
    private Vector3 _target;

    private float _time;
    public float time;

    private float _timeCast;

    private bool _attack;

    public int damage;

    public float firerate;
    private float nextfire = 0f;

    private int _index;

    [Header("Particle")]
    public GameObject ghostParticle;
    public GameObject spiritParticle;
    public float speedParticle;
    public ParticleSystem charge;
    private bool _canMove;
    private bool _canInit;
    private Vector3 _targetParticle;

    [Header("FeedBack")]
    public GameObject laser;
    // public Transform player;
    private bool shoot = true;
    private bool _canShoot;

    private float _timeShoot;
    private bool _goAttack;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
        speedInitial = speed;
        
    }

    void Start()
    {
        arrow = player.pAttack.arrow;
        _c = arrow.GetComponent<SpriteRenderer>().color;
        Debug.Log(_c);
    }


    void Update()
    {
        if (player.gmg._end == true)
            return;
        if (_canInit == true)
        {
            arrow.transform.eulerAngles = Vector3.MoveTowards(arrow.transform.eulerAngles, _target, speed * Time.deltaTime);
            if (arrow.transform.localEulerAngles.z >= angle.y)
            {

                _target = new Vector3(0, 0, angle.x);
            }

            else if (arrow.transform.eulerAngles.z <= angle.x)
            {

                _target = new Vector3(0, 0, angle.y);
            }

            //TIR

            if ((Input.GetButtonDown("Attack_" + player.playerId) || Input.GetButtonDown("Jump_" + player.playerId)) && shoot == true && player.gmg._end == false)
            {
                laser.SetActive(true);
                nextfire = Time.time + firerate  + castTime;
                charge.Play();
                speed = 0;
                shoot = false;
                _goAttack = true;
                _timeShoot = Time.time;
            }

            if (Time.time - _timeShoot > castTime && _goAttack)
            {
                Shoot();
                _goAttack = false;
            }

            //RECHARGE

            if (shoot == false && Time.time > nextfire)
            {
                shoot = true;
                arrow.GetComponent<SpriteRenderer>().color = _c;
            }

            if (_attack && Time.time - _time > time)
            {
                arrow.transform.GetChild(0).gameObject.SetActive(false);
                _attack = false;
                speed = speedInitial;
            }

            if (_goAttack)
            {
                Color c = laser.GetComponent<SpriteRenderer>().color;
                laser.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a += 0.02f);
            }
        }
        
        if (_canMove)
        {
            spiritParticle.transform.position = Vector3.MoveTowards(spiritParticle.transform.position, _targetParticle, speedParticle);
        }

        if (_canMove && spiritParticle.transform.position == _targetParticle)
        {
            
            _canMove = false;
            _canInit = true;
            player.pMovement.mummy.SetActive(true);
            transform.position = player.gmg.eyesPosition[_index].position;
            

            arrow.transform.localScale = new Vector3(12, 12, 0);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.eulerAngles = new Vector3(0, 0, angle.x);

            if (_index < 2)
            {
                arrow.transform.localScale = new Vector3(arrow.transform.localScale.x * -1, arrow.transform.localScale.y, 0);
            }

           // spiritParticle.SetActive(false);
            ghostParticle.SetActive(false);
        }
    }

    void Shoot()
    {
        laser.SetActive(false);
        laser.GetComponent<SpriteRenderer>().color = new Color(laser.GetComponent<SpriteRenderer>().color.r, laser.GetComponent<SpriteRenderer>().color.g, laser.GetComponent<SpriteRenderer>().color.b, 0);
        _time = Time.time;
        _attack = true;
        arrow.transform.GetChild(0).gameObject.SetActive(true);
        arrow.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = player.color;
        arrow.GetComponent<SpriteRenderer>().color = Color.white;
        
        shoot = false;
    }

    public void SetPosition()
    {
        _target = new Vector3(0, 0, 90);
        _canShoot = true;
        _canInit = false;
        _goAttack = false;
        player.pMovement.mummy.SetActive(false);
        _attack = false;
        _target = new Vector3(0, 0, angle.y);
        speed = speedInitial;
        int i = Random.Range(0, 4);

        while (checkPosition(i) != true)
        {
            i = Random.Range(0, 4);
        }

        player.gmg.indexEye.Add(i);

        //ghostParticle.SetActive(true);
        ghostParticle.GetComponent<ParticleSystem>().Play();
        //spiritParticle.SetActive(true);
        spiritParticle.GetComponent<ParticleSystem>().Play();

        _index = i;
        _targetParticle = player.gmg.eyesPosition[_index].position;
        _canMove = true;
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