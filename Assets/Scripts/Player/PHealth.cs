using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PHealth : MonoBehaviour
{
    public Color[] health;
    public TextMesh text;

    public AudioClip hurtSound;
    public AudioClip dieSound;

    [Header("FeedBack")]
    public GameObject hit;
    public float timeInvicible;
    private float _timeInvincible;
    private bool _hit;
    public float clignote;
    private float _clignote;
    private bool _white;
    private Color _color;

    private int _id;
    private PlayerManager player;

    [HideInInspector]
    public List<int> killer = new List<int>();

    void Start ()
    {
       
        player = GetComponent<PlayerManager>();
        _color = player.color;
    }
	
    void Update()
    {
        text.text = player.health.ToString();
        
        if (player.health <= 15)
        {
            text.color = health[2];
        }
        else if (player.health > 15 && player.health <= 25)
        {
            text.color = health[1];
        }
        else
        {
            text.color = health[0];
        }


        if (_hit == true)
        {
            if (Time.time - _timeInvincible > timeInvicible)
            {
                _hit = false;
                player.pMovement.mummy.GetComponent<SpriteRenderer>().color = _color;
            }
            else
                Clignote();
        }
    }

    public void SetHealth(int i, int id)
    {
        if (_hit || player.health <= 0)
            return;

        if (id != 0)
        {
            _timeInvincible = Time.time;
            _hit = true;
            hit.GetComponent<Animator>().SetTrigger("hit");
        }

        _id = id;
        player.health += i;
        player.health = (player.health > 100) ? 100 : player.health;
        
        SetColor();
        if (player.health <= 0)
            Die();
        else if (id != 0)
            player.GetComponent<AudioSource>().PlayOneShot(hurtSound);
    }

    void SetColor()
    {
        
    }

    void Die()
    {
        player.GetComponent<AudioSource>().PlayOneShot(dieSound);
        player.health = 0;
        if (_id != 9)
            killer.Add(_id);
        player.gmg.CheckDeath();
        // CHANGER ICI
        transform.position = new Vector3(100, 100, 100);
        //player.SetAvaibleScript(3);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "eye")
        {
            SetHealth(-50, 9);
        }
    }

    void Clignote()
    {
        if (Time.time - _clignote > clignote)
        {
            if (_white)
                player.pMovement.mummy.GetComponent<SpriteRenderer>().color = new Color(_color.r, _color.g, _color.b, 0.25f);
            else
                player.pMovement.mummy.GetComponent<SpriteRenderer>().color = _color;

            _white = !_white;
            _clignote = Time.time;
        }
    }
}
