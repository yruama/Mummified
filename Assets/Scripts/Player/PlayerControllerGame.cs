using UnityEngine;
using System.Collections;

public class PlayerControllerGame : MonoBehaviour
{
    [Header("Jump")]
    float jumpVelocity = 8;
    public float JumpHeight = 3;
    public float forceFirstJump = .4f;
    public float forceSecondJump = 0.3f;
    bool _canDoubleJump;
    bool _canJump;
    float _timeJump;
    public float timeJump;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;
    Vector3 velocity;
    float gravity = -20;

    [Header("Movement")]
    public float speed = 5;
    bool _facing = true;
    bool _flip = true;
    Controller2D controller;

    [Header("Attaque")]
    public GameObject projectile;
    public GameObject arrow;
    public Vector2 distance;
    private bool _isAttacking;
    private bool _canAttack;
    public Transform[] pos;
    int posIndex;

    [Header("Autre")]
    public int playerId;
    private int _currentKill;
    public int health = 100;
    public SpawnPositionData spawnData;
    public GameManagerGame gmg;
    private Color _color;
    public TextMesh t;

    void Start ()
    {
        
        controller = GetComponent<Controller2D>();
        gravity = -(2 * JumpHeight) / Mathf.Pow(forceFirstJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * forceFirstJump;
        transform.position = spawnData.map1[playerId - 1];

        _canAttack = true;

        _color = GetComponent<PlayerControllerMenu>().GetColor();
        arrow.GetComponent<SpriteRenderer>().color = _color;
        GetComponent<SpriteRenderer>().color = _color;
        GetComponent<PlayerControllerMenu>().enabled = false;
    }
	
	void Update ()
    {
        t.text = health.ToString() + "%";
        if (health <= 15)
        {
            t.color = Color.red;
        }
        else
        {
            t.color = Color.white;
        }

        if (health <= 0)
            return;
        
        Vector2 input;
        if (Input.GetButton("Attack_" + playerId) && health > 15 && _canAttack)
        {
            _isAttacking = true;
            input = new Vector2(Input.GetAxis("Horizontal_" + playerId), Input.GetAxis("Vertical_" + playerId));
            SetArrowPosition((int)(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg));
        }

        if (Input.GetButtonUp("Attack_" + playerId) && health > 15 && _isAttacking == true)
        {
            Attack();
            ResetAttack();
            _canAttack = true;
        }
    }

    void FixedUpdate()
    {
       
        if (health <= 0)
            return;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (_isAttacking == true)
        {
            input.x = 0;
        }

        if (Input.GetButtonDown("Jump_" + playerId))
        {
            if (controller.collisions.below)
            {
                velocity.y = jumpVelocity;
                _canDoubleJump = true;
                _timeJump = Time.time;
            }
            else if (Time.time - _timeJump > timeJump && _canDoubleJump)
            {
                velocity.y = forceSecondJump;
                _canDoubleJump = false;
                _canJump = false;
            }
        }

        float targetVelocityX = input.x * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);
    }

    public void SetHealth(int i, int id)
    {
        health += i;
        health = (health > 100) ? 100 : health;
      

        if (health <= 0)
        {
            health = 0;
            gmg.CheckDeath();
        }
    }

    public void SetArrowPosition(int i)
    {
        if (i > -23 && i <= 23)
            posIndex = 0;
        else if (i > 23 && i <= 68)
            posIndex = 1;
        else if (i > 68 && i <= 113)
            posIndex = 2;
        else if (i > 113 && i <= 158)
            posIndex = 3;
        else if (i > -158 && i <= -113)
            posIndex = 4;
        else if (i > -113 && i <= -68)
            posIndex = 5;
        else if (i > -68 && i <= -23)
            posIndex = 6;
        else if (i > -158 && i <= -179 || i > 158 && i <= 180)
        {
            ResetAttack();
            return;
        }

        arrow.transform.position = pos[posIndex].position;
        arrow.transform.eulerAngles = pos[posIndex].eulerAngles;
    }

    public void ResetAttack()
    {
        _canAttack = true;
        _isAttacking = false;
        arrow.transform.position = new Vector3(100, 100, 100);
    }

    void Attack()
    {
        GameObject g = gmg.bandage[0];
        gmg.bandage.RemoveAt(0);

        g.transform.position = pos[posIndex].position;
        // g.transform.eulerAngles = pos[posIndex].eulerAngles;
        g.GetComponent<Bandage>().p = gameObject;
        g.GetComponent<Bandage>().enabled = true;
        g.GetComponent<Bandage>().Reset();
        g.GetComponent<Bandage>().direction = pos[posIndex].eulerAngles;
        SetHealth(-15, 9);
    }
}
