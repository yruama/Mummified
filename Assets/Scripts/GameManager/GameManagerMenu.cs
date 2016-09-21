using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerMenu : MonoBehaviour
{
    public Sprite[] cadre;
    public Color[] colors;
    public string[] colorsName;
    public GameObject[] players;
    public GameObject menu;

    public GameObject Filtre;

    private int _nbPlayer;
    private int _lastNbPlayer;



    [HideInInspector]
    public int[] availableColors;

	void Start ()
    {
        _lastNbPlayer = -1;
        availableColors = new int[colors.Length];
	}
	
	void Update ()
    {
        GetNumberOfJoysticks();
        if (_nbPlayer != _lastNbPlayer)
        {
            _lastNbPlayer = _nbPlayer;
            SetPlayer();
        }
    }

    public void LockColor(int i)
    {
        availableColors[i] = -1;

        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerControllerMenu>().CheckColor();
        }
    }

    public void CheckValidate()
    {
        int i = 0;

        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerControllerMenu>().GetValidate() == true)
                i += 1;
        }

        if (i == _nbPlayer && _nbPlayer >= 2)
        {
            Debug.Log("ON COMMENCE LA PARTIE");
            StartGame();
            Filtre.SetActive(false);
        }
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
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerControllerMenu>().SetAvailaible(_nbPlayer);
        }
    }

    void StartGame()
    {
        int i = 0;

        while (i < _nbPlayer)
        {
            players[i].GetComponent<PlayerControllerGame>().enabled = true;
            GetComponent<GameManagerGame>().enabled = true;
            GetComponent<GameManagerGame>().nbPlayer = _nbPlayer;
            menu.SetActive(false);
            i++;
        }
    }
}
