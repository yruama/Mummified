using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public PMovement pMovement;
    public PJump pJump;
    public PHealth pHealth;
    public PAttack pAttack;
    public PAnimations pAnimations;
    public Controller2D controller;
    public PMenu pMenu;
    public PEye pEye;

    [HideInInspector]
    public Animator anim;

    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public Vector2 input = Vector3.zero;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool lookAt; // 0 = right - 1 = left
    public int health;
    public Color color;
    public int statut = 1;

    public bool available;

    public int playerId;
    public GameManagerGame gmg;
    public GameManagerMenu gmm;

    /*
     * 1 = Menu
     * 2 = Momie
     * 3 = Eye
     */

    void Start()
    {
        lookAt = false;
        anim = pMovement.mummy.GetComponent<Animator>();
        SetAvaibleScript(1);
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));
        if (health > 0)
        {
            if (isAttacking)
                velocity.x = 0;
            controller.Move(velocity * Time.deltaTime, input);
        }
    }

    public void SetAvaibleScript(int i)
    {
        statut = i;
        if (statut == 1)
        {
            pMenu.enabled = true;
            pMovement.enabled = false;
            pHealth.enabled = false;
            pJump.enabled = false;
            pAttack.enabled = false;
            pEye.enabled = false;
        }
        else if (statut == 2)
        {
            pEye.enabled = false;
            pMenu.enabled = false;
            pMovement.enabled = true;
            pHealth.enabled = true;
            pJump.enabled = true;
            pAttack.enabled = true;
            pMovement.mummy.GetComponent<SpriteRenderer>().color = color;
            pAttack.arrow.GetComponent<SpriteRenderer>().color = color;
            transform.position = gmm.maps[gmm.mapIndex].GetComponent<SpawnPositionData>().player[playerId - 1];
        }
        else if (statut == 3)
        {
            pMenu.enabled = false;
            pMovement.enabled = false;
            pHealth.enabled = false;
            pJump.enabled = false;
            pAttack.enabled = false;
            pEye.enabled = true;
        }
        else
        {
            Debug.LogError("Player Statut : Error !");
        }
    }

    public void Reset()
    {
        if (!available)
            return;

        transform.localScale = new Vector3(2, 2, 0);
        pAttack.arrow.transform.SetParent(null);
        pAttack.arrow.transform.position = new Vector3(100, 100, 100);
        SetAvaibleScript(2);
        
        transform.position = gmm.maps[gmm.mapIndex].GetComponent<SpawnPositionData>().player[playerId - 1];
        health = 100;
    }
}
