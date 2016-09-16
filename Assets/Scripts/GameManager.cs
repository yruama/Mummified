using UnityEngine;
using System.Collections;

public enum menuState
{
    MENU,
    OPTIONS,
    QUITTER,
    PERSONNAGE,
    GAME
};

public class GameManager : MonoBehaviour
{
    [Header("Position")]
    public Transform[] startPlayerPosition;
    public Transform[] utilityPosition;

    [Header("Other")]
    private menuState ms;
    public int i;

    [Header("Menus")]
    public GameObject menu;
    public Transform[] positionMenu;
    public float speedMenu;
    private int _indexMenu;
    private bool _changeMenu;
         
	void Start ()
    {
        ms = menuState.MENU;
        _changeMenu = false;
	}
	
	void Update ()
    {
	    if (_changeMenu == true)
        {
            menu.transform.position = Vector3.MoveTowards(menu.transform.position, positionMenu[_indexMenu].position, speedMenu * Time.deltaTime);
        }
        if (menu.transform.position == positionMenu[_indexMenu].position)
        {
            _changeMenu = false;
        }

        if (Input.GetButtonDown("Back_1"))
        {

        }
	}


    public void SetMenu(int i)
    {
        _indexMenu = i;
        _changeMenu = true;
    }
}
