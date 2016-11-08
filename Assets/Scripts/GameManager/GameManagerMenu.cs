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
    public GameObject[] maps;
    private int _indexMap;

    public Menu m;
    public int mapIndex = 0;

    public bool _isStart;

    public GameObject Filtre;

    private int _nbPlayer;
    private int _lastNbPlayer;

    public Text Round;
    public int roundIndex;
    public int[] rounds;
    public int nbRound;

    public AudioClip[] clip;

    [HideInInspector]
    public int[] availableColors;

	void Start ()
    {
        _isStart = false;
       
        GetComponent<AudioSource>().PlayOneShot(clip[0]);
        
        roundIndex = 0;
        nbRound = rounds[roundIndex];
        //Cursor.visible = false;
        _lastNbPlayer = -1;
        availableColors = new int[colors.Length];
	}
	
	void Update ()
    {
        GetNumberOfJoysticks();
        if (_nbPlayer != _lastNbPlayer && m.currentMenu != 1)
        {
            _lastNbPlayer = _nbPlayer;
            SetPlayer();
        }

        Round.text = rounds[roundIndex].ToString();
        nbRound = rounds[roundIndex];

        if (Input.GetButtonDown("mapRight"))
        {
            GetComponent<AudioSource>().PlayOneShot(clip[2]);
            roundIndex += 1;

            if (roundIndex > rounds.Length - 1)
            {
                roundIndex = 0;
            }
        }
        
        if (Input.GetButtonDown("mapLeft"))
        {
            GetComponent<AudioSource>().PlayOneShot(clip[2]);
            
            roundIndex -= 1;

            if (roundIndex < 0)
            {
               roundIndex = rounds.Length - 1;
                
            }
        }
    }

    public void LockColor(int i)
    {
        availableColors[i] = -1;

        foreach (GameObject p in players)
        {
            p.GetComponent<PMenu>().CheckColor();
        }
    }

    public void CheckValidate()
    {
        int i = 0;

        foreach (GameObject p in players)
        {
            if (p.GetComponent<PMenu>().GetValidate() == true)
                i += 1;
        }

        if (i == _nbPlayer/* && _nbPlayer >= 2*/)
        {
            // return;
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(clip[1]);
            GetComponent<AudioSource>().pitch = 0.9f;
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
            p.GetComponent<PMenu>().SetAvailaible(_nbPlayer);
        }
    }

    void StartGame()
    {
        _isStart = true;
        int i = 0;

        while (i < _nbPlayer)
        {
            players[i].GetComponent<PlayerManager>().SetAvaibleScript(2);
            GetComponent<GameManagerGame>().enabled = true;
            GetComponent<GameManagerGame>().nbPlayer = _nbPlayer;
            menu.SetActive(false);
            i++;
        }
    }
}
