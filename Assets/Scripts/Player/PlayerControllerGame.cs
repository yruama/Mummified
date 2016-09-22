using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class PlayerControllerGame : MonoBehaviour
{
    [Header("Jump")]
    float jumpVelocity = 8;
    public float JumpHeight = 3;
    public float forceFirstJump = .4f;
    public float forceSecondJump = 0.3f;
    public int numberJump = 2;
    bool _canDoubleJump;
    bool _firstJump;
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
    private Animator _anim;
    public GameObject mummieSprite;

    [Header("FeedBack")]
    public GameObject hit;
    public float timeInvicible;
    private float _timeInvincible;
    private bool _hit;
    public float clignote;
    private float _clignote;
    private bool _white;

    [Header("Vibration")]
    public PlayerIndex playerIndex;
    public float timeToVibrate;
    public float vibrateForceHit;
    public float vibrateForAttack;
    private float _vibrateForce;
    private float _timeVibrate;
    private bool _vibrate;

    [HideInInspector]
    public List<int> killer = new List<int>();

    void Start ()
    {
        _hit = false;
        _white = false;
        controller = GetComponent<Controller2D>();
        gravity = -(2 * JumpHeight) / Mathf.Pow(forceFirstJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * forceFirstJump;
        transform.position = spawnData.map1[playerId - 1];

        _anim = mummieSprite.GetComponent<Animator>();
        _canAttack = true;

        _color = GetComponent<PlayerControllerMenu>().GetColor();
        arrow.GetComponent<SpriteRenderer>().color = _color;
        mummieSprite.GetComponent<SpriteRenderer>().color = _color;
        GetComponent<PlayerControllerMenu>().enabled = false;
    }

    public void Reset()
    {
        if (GetComponent<PlayerControllerGame>().enabled == true)
        {
            _canAttack = true;
            health = 100;
            _hit = false;
            _white = false;
            transform.position = spawnData.map1[playerId - 1];
        }

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

        if (_hit == true)
        {
            if (Time.time - _timeInvincible > timeInvicible)
            {
                _hit = false;
                mummieSprite.GetComponent<SpriteRenderer>().color = _color;
            }
            else        
                Clignote();
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));

        if (Input.GetButton("Attack_" + playerId) && health > 15 && _canAttack)
        {
            _anim.SetBool("prepareAttack", true);
            _isAttacking = true;
            if (input == Vector2.zero)
            {
                if (!_facing)
                {
                    input = Vector2.right;
                }
                else
                {
                    input = Vector2.left;
                }
            }
            SetArrowPosition((int)(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg));
        }

        if (Input.GetButtonUp("Attack_" + playerId) && health > 15)
        {
            if (_isAttacking)
            {
                Attack();
                ResetAttack();
            }
            _anim.SetTrigger("ajustAttack");
            _anim.SetBool("prepareAttack", false);
            _canAttack = true;
        }

       
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            _firstJump = false;
        }

        if (_isAttacking == true)
        {
            input.x = 0;
        }

        /* *** TRY JUMP ***/
        if (Input.GetButtonDown("Jump_" + playerId))
        {
            if (controller.collisions.below)
            { 
                velocity.y = jumpVelocity;
                _timeJump = Time.time;
                _firstJump = true;
            }
            else if (Time.time - _timeJump > timeJump && _canDoubleJump)
            {
                velocity.y = forceSecondJump;
                _canDoubleJump = false;
                _firstJump = false;
            }
        }

        if (Input.GetButtonUp("Jump_" + playerId) && _firstJump == true)
        {
            _canDoubleJump = true;
        }
        /* *** END JUMP *** */
        if (Mathf.Abs(input.x) > 0)
        {
            _anim.SetBool("walk", true);
        }
        else
        {
            _anim.SetBool("walk", false);
        }

        float targetVelocityX = input.x * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        if (health <= 0)
        {
            velocity = new Vector3(0, -9.81f, 0);
        }

        if ((input.x > 0) && _facing)
            Flip();
        else if ((input.x < 0) && !_facing)
            Flip();

        controller.Move(velocity * Time.deltaTime, input);
    }

    void FixedUpdate()
    {
       if (Time.time - _timeVibrate > timeToVibrate)
        {
            _vibrate = false;
            gmg.GetComponent<XInputTestCS>().SetVibration(playerIndex, 0.0f, 0.0f);
        }

        if (_vibrate)
        {
            gmg.GetComponent<XInputTestCS>().SetVibration(playerIndex, _vibrateForce, _vibrateForce);
        }

    }

    public void SetHealth(int i, int id)
    {
        if (_hit && id < 5)
            return;
        health += i;
        health = (health > 100) ? 100 : health;

        if (id != playerId && id < 5)
        {
            _timeInvincible = Time.time;
            _hit = true;
            hit.GetComponent<Animator>().SetTrigger("hit");

            _vibrateForce = vibrateForceHit;
            _vibrate = true;
            _timeVibrate = Time.time;
        }

        if (health <= 0)
        {
            health = 0;

            if (id < 5)
            {
                killer.Add(id);
            }
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
            if (!controller.collisions.below)
            {
                posIndex = 7;
            }
            else
            {
                _canAttack = false;
                ResetAttack();
                return;
            }
        }

        arrow.transform.position = pos[posIndex].position;
        arrow.transform.eulerAngles = pos[posIndex].eulerAngles;
    }

    public void ResetAttack()
    {
       // _canAttack = true;
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
        g.GetComponent<Bandage>().playerId = playerId;
        g.GetComponent<Bandage>().direction = pos[posIndex].eulerAngles;
        g.GetComponent<Bandage>().Reset(0);

        _vibrateForce = vibrateForAttack;
        _vibrate = true;
        _timeVibrate = Time.time;

        SetHealth(-15, 9);
    }

    void LateUpdate()
    {

    }

    void Flip()
    {
        if (_flip == true)
        {
            _facing = !_facing;
            mummieSprite.transform.localScale = new Vector3(mummieSprite.transform.localScale.x * -1, mummieSprite.transform.localScale.y, mummieSprite.transform.localScale.z);
        }
    }

    void Clignote()
    {
        if (Time.time - _clignote > clignote)
        {
            if (_white)
                mummieSprite.GetComponent<SpriteRenderer>().color = new Color(_color.r, _color.g, _color.b, 0.25f);
            else
                mummieSprite.GetComponent<SpriteRenderer>().color = _color;

            _white = !_white;
            _clignote = Time.time;
        }
    }
}
