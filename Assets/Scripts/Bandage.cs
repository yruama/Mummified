using UnityEngine;
using System.Collections;

public class Bandage : MonoBehaviour
{
    
    public float gravityForce;
    public float startForce;
    public float speed = 5;
    public float timeToUseGravity;

    private float _time;


    private float _gravity;
    private Vector3 _velocity;
    private int _state;
    Controller2D _c;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    
    float velocityXSmoothing;

    public Vector3 direction;

    bool _test = true;

    void Start()
    {
        _time = Time.time;
        _c = GetComponent<Controller2D>();
        _gravity = -9.81f;
        _velocity = new Vector2(DegreeToVector2(direction.z).y * -1, DegreeToVector2(direction.z).x) * startForce;
    }

    void Update()
    {
        if (_c.collisions.above || _c.collisions.below)
        {
            _state = 1;
        }

        if (_state == 0)
        {
            _velocity.x = Mathf.SmoothDamp(_velocity.x, _velocity.x, ref velocityXSmoothing, (_c.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            if (Time.time - _time > timeToUseGravity)
                _velocity.y += _gravity * Time.deltaTime;
            _c.Move(_velocity * Time.deltaTime, Vector3.zero);
        }
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
        Debug.Log(coll.gameObject.name);
        if (coll.gameObject.tag == "Player" && _state == 0)
        {
            coll.gameObject.GetComponent<Player>().SetHealth(-25);
            Destroy(gameObject);
        }

        if (coll.gameObject.tag == "Player" && _state == 1)
        {
            coll.gameObject.GetComponent<Player>().SetHealth(10);
            Destroy(gameObject);
        }
    }
}
