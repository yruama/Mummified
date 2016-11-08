using UnityEngine;
using System.Collections;

public class PAttack : MonoBehaviour
{
    private PlayerManager player;

    public GameObject projectile;
    public GameObject arrow;
    public Vector2 distance;
    private bool _canAttack;
    public Transform[] pos;
    public AudioClip attackSound;
    int posIndex;


    void Start()
    {
        _canAttack = true;
        player = GetComponent<PlayerManager>();
    }

	void Update ()
    {

        if (Input.GetButton("Attack_" + player.playerId) && player.health > 15 && _canAttack)
        {
            player.anim.SetBool("prepareAttack", true);
            player.isAttacking = true;
            if (player.input == Vector2.zero)
            {
                if (player.lookAt == true)
                {
                    player.input = Vector2.right;
                }
                else
                {
                    player.input = Vector2.left;
                }
            }
            SetArrowPosition((int)(Mathf.Atan2(player.input.x, player.input.y) * Mathf.Rad2Deg));
        }

        if (Input.GetButtonUp("Attack_" + player.playerId) && player.health > 15)
        {
            if (player.isAttacking)
            {
                Attack();
                ResetAttack();
            }
            _canAttack = true;
        }

        if (player.isAttacking == true && player.health <= 15)
        {
            ResetAttack();
            player.isAttacking = false;
        }

    }

    public void ResetAll()
    {
        player.anim.SetTrigger("ajustAttack");
        player.anim.SetBool("prepareAttack", false);
        player.isAttacking = false;
        arrow.transform.position = new Vector3(100, 100, 100);
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
            if (!player.controller.collisions.below)
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
        player.anim.SetTrigger("ajustAttack");
        player.anim.SetBool("prepareAttack", false);
        player.isAttacking = false;
        arrow.transform.position = new Vector3(100, 100, 100);
    }

    void Attack()
    {
        player.GetComponent<AudioSource>().PlayOneShot(attackSound);
        GameObject g = player.gmg.bandage[0];
        player.gmg.bandage.RemoveAt(0);

        g.transform.position = pos[posIndex].position;
        // g.transform.eulerAngles = pos[posIndex].eulerAngles;
        g.GetComponent<Bandage>().p = gameObject;
        g.GetComponent<Bandage>().enabled = true;
        g.GetComponent<Bandage>().playerId = player.playerId;
        g.GetComponent<Bandage>().direction = pos[posIndex].eulerAngles;
        g.GetComponent<Bandage>().Reset(0);
        g.GetComponent<SpriteRenderer>().color = player.color;

        player.pHealth.SetHealth(-15, 0);
    }
}