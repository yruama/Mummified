using UnityEngine;
using System.Collections;

public class PMovement : MonoBehaviour
{
    private PlayerManager player;
    private bool _facing = true;
    private bool _flip = true;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float velocityXSmoothing;

    public Sprite Jump;
    public Sprite Idle;

    public GameObject mummy;

    public float speed = 5;

    void Start ()
    {
        player = GetComponent<PlayerManager>();
    }
	
	void Update ()
    {
        float targetVelocityX = player.input.x * speed;
        player.velocity.x = Mathf.SmoothDamp(player.velocity.x, targetVelocityX, ref velocityXSmoothing, (player.controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if (!player.controller.collisions.below)
        {
            player.anim.SetBool("walk", false);
            player.anim.SetBool("jumping", true);
        }
        else
        {
            player.anim.SetBool("jumping", false);
            if (Mathf.Abs(player.input.x) > 0)
            {
                player.anim.SetBool("walk", true);
            }
                
            else
            {
                player.anim.SetBool("walk", false);
            }
                
        }

        

        if ((player.input.x > 0) && _facing)
            Flip();
        else if ((player.input.x < 0) && !_facing)
            Flip();
    }

    void Flip()
    {
        if (_flip == true)
        {
            _facing = !_facing;
            player.lookAt = !player.lookAt;
            mummy.transform.localScale = new Vector3(mummy.transform.localScale.x * -1, mummy.transform.localScale.y, mummy.transform.localScale.z);
        }
    }
}
