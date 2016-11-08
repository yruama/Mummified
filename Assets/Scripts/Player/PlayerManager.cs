using UnityEngine;
using UnityEngine.UI;
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

    [Header("Data")]
    public int[] hitBandelette;
    public int[] hitLaser;
    public int throwBandeletteNb;
    public int healBandeletteNb;
    public GameObject Score;
    public int score;

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

    public int ScoreMalus = 0;

    public bool _canJump;

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
        score = 0;

        pMenu.enabled = true;
        pMovement.enabled = false;
        pHealth.enabled = false;
        pJump.enabled = false;
        pAttack.enabled = false;
        pEye.enabled = false;

    }

    void Update()
    {
        /* * SCORE * */
        if (score < 0)
            score = 0;
        Score.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
        /* *** */


        input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));

        if (gmm._isStart)
        {
            if (gmg._DecompteBool == true)
            {
                pAttack.ResetAll();
                pAttack.enabled = false;
                pMovement.enabled = false;
            }
            else
            {
                pAttack.enabled = true;
                pMovement.enabled = true;
            }
        }
        

        Debug.Log(_canJump);

        if (gmg._DecompteBool == true || gmg._end == true)
        {
            velocity.x = 0;
            input.x = 0;
        }

        if (health > 0)
        {
            if (isAttacking)
                velocity.x = 0;
            if (gmg.GetComponent<Pause>().pause == false)
            controller.Move(velocity * Time.deltaTime, input);
        }
    }

    public void SetAvaibleScript(int i)
    {
        statut = i;
        if (statut == 1)
        {
            Score.SetActive(false);
            pMenu.enabled = true;
            pMovement.enabled = false;
            pHealth.enabled = false;
            pJump.enabled = false;
            pAttack.enabled = false;
            pEye.enabled = false;
        }
        else if (statut == 2)
        {
            /* SCORE */
            Score.SetActive(true);
            Score.GetComponent<Image>().color = color;
            /* ** */
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
            pEye.SetPosition();
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

        pHealth._die = false;
        ScoreMalus = 0;
        pAttack.arrow.transform.SetParent(null);
        pAttack.arrow.transform.localScale = new Vector3(7.5f, 7.5f, 0);
        pAttack.arrow.transform.localScale = new Vector3(7.5f, 7.5f, 0);
        pAttack.arrow.transform.position = new Vector3(100, 100, 100);
        pEye.spiritParticle.transform.position = Vector2.zero;
        pEye.spiritParticle.SetActive(false);
        pEye.ghostParticle.transform.position = Vector2.zero;
        pEye.ghostParticle.SetActive(false);
        pHealth.text.gameObject.SetActive(true);
        pAttack.arrow.transform.GetChild(0).gameObject.SetActive(false);
        pAttack.arrow.transform.GetChild(1).gameObject.SetActive(false);
        pEye.charge.Stop();
        pMovement.mummy.SetActive(true);
        SetAvaibleScript(2);
        
        transform.position = gmm.maps[gmm.mapIndex].GetComponent<SpawnPositionData>().player[playerId - 1];
        health = 100;
    }
}
