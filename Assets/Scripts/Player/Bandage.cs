using UnityEngine;
using System.Collections;

public class Bandage : MonoBehaviour
{
    public Sprite h;
    public Sprite s;
    public GameManagerGame gmg;
    public int playerId;
    public float gravityForce;
    public float startForce;
    public float speed = 5;
    public float timeToUseGravity;
    public float frottementdelair;
    private float _time;
    private bool _useGravity;

    private float _gravity;
    private Vector3 _velocity;
    private int _state;
    Controller2D _c;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    
    float velocityXSmoothing;

    public Vector3 direction;

    [HideInInspector]
    public GameObject p;

    bool _test = true;

    public void Reset(int i)
    {
        _c.collisions.above = false;
        _c.collisions.below = false;
        _c.collisions.left = false;
        _c.collisions.right = false;
        _state = i;
        _useGravity = true;
        GetComponent<SpriteRenderer>().sprite = s;
        _time = Time.time;
        _gravity = -9.81f;
        _velocity = new Vector2(DegreeToVector2(direction.z).x, DegreeToVector2(direction.z).y) * startForce;
    }

    void Start()
    {
        _c = GetComponent<Controller2D>();
    }

    void Update()
    {
        if (_c.collisions.above || _c.collisions.below || _c.collisions.left || _c.collisions.right || _state == 1)
        {
            _velocity.y = -15f;
            _velocity.x = 0;
            _useGravity = false;
        }

        if (_c.collisions.below)
        {
            _state = 1;
            
        }

        if (_state == 1)
            GetComponent<SpriteRenderer>().sprite = h;

        else if (_useGravity)
        {
            _velocity.x = Mathf.SmoothDamp(_velocity.x, _velocity.x, ref velocityXSmoothing, (_c.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            if (Time.time - _time > timeToUseGravity && _useGravity)
            {
                _velocity.y += _gravity * frottementdelair;
            }
                
            
        }
        _c.Move(_velocity * Time.deltaTime, Vector3.zero);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (p != coll.gameObject)
        {
            if (coll.gameObject.tag == "Player" && _state == 0)
            {
                coll.gameObject.GetComponent<PlayerControllerGame>().SetHealth(-25, playerId);
                gmg.bandage.Add(gameObject);
                transform.position = new Vector3(100, 100, 100);
                GetComponent<Bandage>().enabled = false;
            }
        }


        if (coll.gameObject.tag == "Player" && _state == 1)
        {
            coll.gameObject.GetComponent<PlayerControllerGame>().SetHealth(10, 9);
            gmg.bandage.Add(gameObject);
            transform.position = new Vector3(100, 100, 100);
            GetComponent<SpriteRenderer>().sprite = s;
            _state = 0;
            GetComponent<Bandage>().enabled = false;
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(100, 100, 100);
    }
}
