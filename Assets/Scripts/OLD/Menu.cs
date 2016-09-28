using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    public GameObject MenuGeneral;
    public GameObject MenuGame;
    public GameObject MenuOptions;
    public GameObject MenuControl;
    public GameObject MenuCredit;
    public float speedMenu;

    public GameObject eventSystem;
    public int currentMenu = 1;
    private int _prepareMenu;

    public Transform endPos;
    public Transform startPos;
    public Transform neutrePos;

    private GameObject _currentMenu;
    private GameObject _newMenu;
    private bool _changeMenu;

	void Start ()
    {
        _currentMenu = MenuGeneral;
        _newMenu = MenuGame;
        _changeMenu = false;
	}
	
	void Update ()
    {
        if (_changeMenu)
        {
            _currentMenu.transform.position = Vector3.MoveTowards(_currentMenu.transform.position, endPos.position, speedMenu * Time.deltaTime);
            _newMenu.transform.position = Vector3.MoveTowards(_newMenu.transform.position, neutrePos.position, speedMenu * Time.deltaTime);
        }

        if (_newMenu.transform.position == neutrePos.position && _changeMenu == true)
        {
            _changeMenu = false;
            _currentMenu.transform.position = startPos.position;
            _currentMenu = _newMenu;
            currentMenu = _prepareMenu;
        }

	}


    public void SetMenu(int i)
    {
        switch (i)
        {
            case 1:
                _newMenu = MenuGeneral;
                break;
            case 2:
                _newMenu = MenuGame;
                break;
            case 3:
                _newMenu = MenuOptions;
                break;
            case 4:
                _newMenu = MenuControl;
                break;
            case 5:
                _newMenu = MenuCredit;
                break;
            case 6:
                Application.Quit();
                break;
            default:
                Debug.LogError("Error Menu");
                break;
        }
        if (i != 1)
            eventSystem.SetActive(false);
        else
            eventSystem.SetActive(true);
        _changeMenu = true;
        _prepareMenu = i;
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
