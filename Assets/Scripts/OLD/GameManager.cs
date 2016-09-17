using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Position")]
    public Transform[] startPlayerPosition;
    public Transform[] utilityPosition;

    [Header("Menus")]
    public GameObject menu;
    public Transform[] positionMenu;
    public float speedMenu;
    private int _indexMenu;
    private int _lastIndexMenu;
    private Vector2[] _targetPostion = new Vector2[2];
    private bool _changeMenu;

    [Header("Color")]
    public List<Color> colors;

    [Header("Player")]
    public GameObject Players;
    private int _nbPlayer;
    private int _lastNbPlayer;
         
	void Start ()
    {
        _lastIndexMenu = 0;
        _changeMenu = false;
	}
	
	void Update ()
    {
        GetNumberOfJoysticks();
        Debug.Log("NBPLAYER : " + _nbPlayer + "LastNBPLAYER : " + _lastNbPlayer);
        if (_nbPlayer != _lastNbPlayer)
        {
            _lastNbPlayer = _nbPlayer;
            SetPlayer();
        }
           

        



        if (_changeMenu == true)
        {
            positionMenu[_indexMenu].position = Vector3.MoveTowards(positionMenu[_indexMenu].position, _targetPostion[0], speedMenu * Time.deltaTime);
            positionMenu[_lastIndexMenu].position = Vector3.MoveTowards(positionMenu[_lastIndexMenu].position, _targetPostion[1], speedMenu * Time.deltaTime);
        }
        if (menu.transform.position == positionMenu[_indexMenu].position && _changeMenu == true)
        {
            positionMenu[_lastIndexMenu].position = positionMenu[4].position;
        }

        if (Input.GetButtonDown("Back"))
        {
            InputBack();
        }
	}


    public void SetMenu(int i)
    {
        _indexMenu = i;
        _targetPostion[0] = positionMenu[_lastIndexMenu].position;
        _targetPostion[1] = positionMenu[3].position;
        _changeMenu = true;
    }

    public void Quitter()
    {

    }

    void InputBack()
    {
        if (_indexMenu == 4)
            SetMenu(3);
        else
            SetMenu(0);
    }

    void GetNumberOfJoysticks()
    {
        int i = 0;
        int j = 0;

        while (Input.GetJoystickNames().Length > i)
        {
            if (Input.GetJoystickNames()[i].Length > 0)
            {
                j++;
            }
            i = i + 1;
        }
        _nbPlayer = j;


    }

    void SetPlayer()
    {
        foreach (Transform child in Players.transform)
        {
            child.gameObject.GetComponent<Player>().SetState(2);
        }

        int i = 0;

        while (i < _nbPlayer)
        {
            Players.transform.GetChild(i).GetComponent<Player>().SetState(0);
            i = i + 1;
        }
    }
}
