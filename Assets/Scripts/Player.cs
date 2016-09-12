using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    /* *** Infos *** */
    public int playerId;

    /* *** Jump & Deplacement *** */
    float jumpVelocity = 8;
    public float speed = 5;
    public float JumpHeight = 3;
    public float timeToJumpApex = .4f;
    int nbJump = 0;
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
        _isAttacking = false;
        _canAttack = true;
        controller = GetComponent<Controller2D>();
        gravity = -(2 * JumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
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
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));

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
            velocity.y = jumpVelocity;
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
        g.transform.eulerAngles = pos[posIndex].eulerAngles;
        SetHealth(-15);
    }
}
