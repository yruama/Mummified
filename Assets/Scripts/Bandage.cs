using UnityEngine;
using System.Collections;

public class Bandage : MonoBehaviour
{
 public float speed;
    public Vector3 originalDirection;
    public int state = 0;
    public Sprite health;
    bool _changeState;
    Rigidbody2D _rb;
    float _time;
    public float gravity = 2;

    void Start ()
    {
        _changeState = false;
        originalDirection = new Vector2(DegreeToVector2(transform.eulerAngles.z).y * -1, DegreeToVector2(transform.eulerAngles.z).x);
        originalDirection.Normalize();
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(originalDirection * speed * 100);
        _time = Time.time;
    }

    void Update()
    {
        if (_changeState == false && GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            _rb.velocity = Vector2.zero;
            _changeState = true;
            state = 1;
            GetComponent<SpriteRenderer>().sprite = health;
        }
        _rb.gravityScale = Mathf.Pow(Time.time - _time, gravity);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll.gameObject.name);
        if (coll.gameObject.tag == "Player" && state == 0)
        {
            coll.gameObject.GetComponent<Player>().SetHealth(-25);
            Destroy(gameObject);
        }

        if (coll.gameObject.tag == "Player" && state == 1)
        {
            coll.gameObject.GetComponent<Player>().SetHealth(10);
            Destroy(gameObject);
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
}
