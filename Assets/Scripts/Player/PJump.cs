using UnityEngine;
using System.Collections;

public class PJump : MonoBehaviour
{
    private PlayerManager player;

    public float JumpHeight = 3;
    public float forceFirstJump = 0.4f;
    public float forceSecondJump = 0.3f;

    float jumpVelocity = 8;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    public int numberJump = 2;

    public AudioClip firstJumpSound;
    public AudioClip secondJumpSound;

    private int _nbJump;

    bool _canDoubleJump;
    bool _firstJump;

    float gravity = -20;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        gravity = -(2 * JumpHeight) / Mathf.Pow(forceFirstJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * forceFirstJump;
        _nbJump = 0;
    }

    void Update ()
    {
        if (player.controller.collisions.above || player.controller.collisions.below)
        {
            if (player.controller.collisions.above)
                _nbJump = 0;
            if (player.controller.collisions.above == false && _nbJump == 0)
                _canDoubleJump = true;
            player.velocity.y = 0;
        }

        if (Input.GetButtonDown("Jump_" + player.playerId))
        {
            if (player.controller.collisions.below)
            {
                player.GetComponent<AudioSource>().PlayOneShot(firstJumpSound);
                player.velocity.y = jumpVelocity;
                _firstJump = true;
            }
            else if (_canDoubleJump)
            {
                player.GetComponent<AudioSource>().PlayOneShot(secondJumpSound);
                player.velocity.y = forceSecondJump;
                _canDoubleJump = false;
                _firstJump = false;
            }
        }

        if (Input.GetButtonUp("Jump_" + player.playerId) && _firstJump == true)
        {
            _canDoubleJump = true;
        }

        player.velocity.y += gravity * Time.deltaTime;
    }
}