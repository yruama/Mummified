using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    /* *** State *** */
    private int _state; //0 = menu 1 = jeu // 2 = afk

    /* *** Menu *** */
    public GameObject myColor;
    public Color c;
    private Color _saveColor;
    public int indexColor;
    public float timeColor;
    private float _timeColor;
    private bool _validate;

    /* *** Infos *** */
    public int playerId;
    public GameManager gm;

    /* *** Jump & Deplacement *** */
    float jumpVelocity = 8;
    public float speed = 5;
    public float JumpHeight = 3;
    public float forceFirstJump = .4f;
    public float forceSecondJump = 0.3f;
    int nbJump = 0;
    float _timeJump;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;
    bool _facing = true;
    bool _flip = true;
    Vector3 velocity;
    float gravity = -20;
    Controller2D controller;

    /* *** Attaque *** */
    public GameObject projectile;
    public GameObject Arrown;
    public Vector2 distance;
    private bool _isAttacking;
    private bool _canAttack;
    public Transform[] pos;
    int posIndex;

    /* *** Point de vie *** */
    public int health = 100;

	void Start ()
    {
        _validate = false;
        _timeColor = Time.time;
        indexColor = playerId - 1;
        c = gm.colors[indexColor];
        _state = 2;
        _timeJump = Time.time;
        _isAttacking = false;
        _canAttack = true;
        controller = GetComponent<Controller2D>();
        gravity = -(2 * JumpHeight) / Mathf.Pow(forceFirstJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * forceFirstJump;
        forceSecondJump = Mathf.Abs(gravity) * forceSecondJump;
    }

    void Update()
    {
        if (_state == 2)
        {
            c = Color.grey;
            myColor.GetComponent<Image>().color = c;
            return;
        }
        if (_state == 0)
        {
            if (!_validate)
                c = gm.colors[indexColor];
            else
                c = _saveColor;
            myColor.GetComponent<Image>().color = c;
            return;
        }
            


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
        if (_state == 2)
            return;

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));

        if (_state == 0)
        {
            if (input.x > 0 && Time.time - _timeColor > timeColor)
            {
                _timeColor = Time.time;
                indexColor += 1;
                if (indexColor > gm.colors.Count - 1)
                    indexColor = 0;
            }
            else if (input.x < 0 && Time.time - _timeColor > timeColor)
            {
                _timeColor = Time.time;
                indexColor -= 1;
                if (indexColor < 0)
                    indexColor = gm.colors.Count - 1;
            }

            if (_validate == false && Input.GetButtonDown("Jump_" + playerId))
            {
                _saveColor = c;
                gm.colors.Remove(c);
                _validate = true;
            }
            else if (_validate == true && Input.GetButtonDown("Attack_" + playerId))
            {
                gm.colors.Add(_saveColor);
                _validate = false;
            }
            return;
        }


        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            nbJump = 0;
        }

        if (_isAttacking == true)
        {
            input.x = 0;
        }


        if (Input.GetButtonDown("Jump_" + playerId) && nbJump < 2)
        {
            if (nbJump == 0)
            {
                velocity.y = jumpVelocity; Debug.Log("1");
            }
            else
            {
                velocity.y = forceSecondJump; Debug.Log("2");
            }
                
            nbJump += 1;
        }


        float targetVelocityX = input.x * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);
    }

    public void SetHealth(int i)
    {
        health += i;
        health = (health > 100) ? 100 : health; 
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

        Arrown.transform.position = pos[posIndex].position;
        Arrown.transform.eulerAngles = pos[posIndex].eulerAngles;
    }

    public void ResetAttack()
    {
        _canAttack = false;
        _isAttacking = false;
        Arrown.transform.position = new Vector3(100, 100, 100);
    }

    void Attack()
    {
        GameObject g = Instantiate(projectile, pos[posIndex].position, Quaternion.identity) as GameObject;
        g.GetComponent<Bandage>().direction = pos[posIndex].eulerAngles;
        SetHealth(-15);
    }

    public void SetState(int i)
    {
        _state = i;
    }
}
