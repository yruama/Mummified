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
    public bool _hit;
    public float clignote;
    private float _clignote;
    private bool _white;
    private Color _color;

    public bool _die;
    private float _timeDie;
    public GameObject ghost;

    private Color _healthColor;
    private Color _saveColor;
    private bool _health;
    private GameObject _healthInfo;
    public GameObject[] healthInfo;

    private int _id;
    private PlayerManager player;

    [HideInInspector]
    public List<int> killer = new List<int>();

    [HideInInspector]
    public List<int> killerL = new List<int>();

    [HideInInspector]
    public List<int> hitterB = new List<int>();

    [HideInInspector]
    public List<int> hitterL = new List<int>();

    private int _idLaser;

    void Start ()
    {
        _die = false;
        player = GetComponent<PlayerManager>();
        _color = player.color;
    }
	
    void Update()
    {
        if (_die == true)
        {
            ghost.SetActive(true);
            if (Time.time - _timeDie > 0.75f)
            {
                ghost.SetActive(false);
                player.SetAvaibleScript(3);
                player.gmg.CheckDeath();
            }
            return;
        }
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

        if (_health)
        {
            _healthInfo.GetComponent<TextMesh>().color = new Color(_healthColor.r, _healthColor.g, _healthColor.b, _healthColor.a -= 0.025f);

            if (_healthColor.a <= 0)
            {
                _health = false;
                
                _healthInfo.SetActive(false);
            }
        }
    }

    public void SetHealth(int i, int id)
    {
        if ((_hit && i == -50) || (_hit &&  i == -25) || (player.health <= 0 && i < 0))
            return;

        if (_healthInfo != null)
        {
            _healthInfo.GetComponent<TextMesh>().color = _saveColor;
            _healthInfo.SetActive(false);
        }
            
        switch (i)
        {
            
            case 10:
                _healthInfo = healthInfo[0];
                healthInfo[0].SetActive(true);
                _healthColor = healthInfo[0].GetComponent<TextMesh>().color;
                _saveColor = _healthColor;
                _health = true;
                player.healBandeletteNb += 1;
                break;
            case -15:
                _healthInfo = healthInfo[1];
                healthInfo[1].SetActive(true);
                _healthColor = healthInfo[1].GetComponent<TextMesh>().color;
                _saveColor = _healthColor;
                _health = true;
                player.throwBandeletteNb += 1;
                break;
            case -25:
                _healthInfo = healthInfo[2];
                healthInfo[2].SetActive(true);
                _healthColor = healthInfo[2].GetComponent<TextMesh>().color;
                _saveColor = _healthColor;
                _health = true;
                hitterB.Add(id);
                break;
            case -50:
                _healthInfo = healthInfo[3];
                healthInfo[3].SetActive(true);
                _healthColor = healthInfo[3].GetComponent<TextMesh>().color;
                _saveColor = _healthColor;
                _health = true;
                hitterL.Add(_idLaser);
                break;
            
        }


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
        {
            if (id == 9)
            {
                killerL.Add(_idLaser);
                player.score -= 1;
            }
            else
            {
                SetScore(id);
            }
            Die();
        }
            
        else if (id != 0)
            player.GetComponent<AudioSource>().PlayOneShot(hurtSound);
    }

    void SetScore(int i)
    {
        player.gmg.Players.transform.GetChild(i - 1).GetComponent<PlayerManager>().score += 1;
    }

    void SetColor()
    {
        
    }

    void Die()
    {
        player.pMovement.mummy.SetActive(false);
        _timeDie = Time.time;
        player.pAttack.ResetAttack();
        player.GetComponent<AudioSource>().PlayOneShot(dieSound);
        player.health = 0;
        if (_id != 9)
            killer.Add(_id);
        _die = true;
        // ATTENTION player.gmg.CheckDeath();
        text.gameObject.SetActive(false);
        

        
        
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "eye")
        {
            _idLaser = coll.transform.parent.parent.GetComponent<PlayerManager>().playerId;
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
