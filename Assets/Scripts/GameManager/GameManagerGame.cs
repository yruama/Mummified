using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class GameManagerGame : MonoBehaviour
{
    [Header("Sprites Data")]
    public Transform bandages;
    [HideInInspector]
    public List<GameObject> bandage;

    [Header("Game Infos")]
    public int nbPlayer;
    public int nbKill;
    private int _currentManche;
    private int _death;
    public GameObject Players;
    private bool _end;
    private bool _endGame;

    public Menu m;

    [Header("Event")]
    public float timeBandageEvent;
    private float _timeBandage;
    public Transform[] eyesPosition;
    public List<int> indexEye;

    [Header("Timer")]
    public float timeToDisplayScore;
    private float _timeScore;
    public float timeToDisplayWinner;
    private float _timeWinner;

    [Header("Score")]
    public GameObject[] playerScore;
    public Text scoreText;
    private int _round;
    public GameObject score;

    [Header("Sound")]
    public AudioClip theme;
    private AudioSource _audio;

    private bool _isPause;

    void Start()
    {
        _isPause = false;
        _endGame = false;
        GetComponent<AudioSource>().enabled = true;
        _round = 1;
        _end = false;
        foreach (Transform b in bandages)
            bandage.Add(b.gameObject);

        _currentManche = 0;
    }

    void Update()
    {
        if (Time.time - _timeBandage > timeBandageEvent && _end == false)
        {
            _timeBandage = Time.time;
            SpawnBandage();
        }

        if (Time.time - _timeScore > timeToDisplayScore && _end == true && _endGame == false)
        {
           
            Debug.Log("Afficher Score");
            _end = false;
            _timeBandage = Time.time;
            SelectMap();
            Reset();
        }

        if (Time.time - _timeWinner > timeToDisplayWinner && _endGame == true)
        {
            GetComponent<GameManagerMenu>().Filtre.SetActive(true);
            m.SetMenu(1);
            m.MenuGame.SetActive(true);
            score.SetActive(false);
            _endGame = false;
            GetComponent<GameManagerGame>().enabled = false;
        }

        if (Input.GetButtonDown("Pause") && _isPause == false)
        {
            _isPause = true;
            Time.timeScale = 0.0f;
        }
        else if (Input.GetButtonDown("Pause") && _isPause)
        {
            _isPause = false;
            Time.timeScale = 1.0f;
        }

    }

    public void CheckDeath()
    {
        _death += 1;

        if (_death + 1 == nbPlayer)
        {
            _end = true;
            DisplayScore();
            _timeScore = Time.time;
            
        }
    }

    void DisplayScore()
    {
        GetComponent<GameManagerMenu>().Filtre.SetActive(true);
        int[] i = new int[4];
        Color[] c = new Color[4];
        int k = 0;

        foreach (Transform t in Players.transform)
        {
            int j = 0;
            c[k] = t.GetComponent<PlayerManager>().color;
            while (j < t.GetComponent<PlayerManager>().pHealth.killer.Count)
            {
                i[t.GetComponent<PlayerManager>().pHealth.killer[j] - 1] += 1;
                j = j + 1;
            }
            k = k + 1;
        }

        k = 0;
        while (k < 4)
        {
            if (i[k] >= nbKill)
            {
                GetComponent<AudioSource>().Stop();
                Debug.Log("C'est FINIT");
                _timeWinner = Time.time;
                _endGame = true;
                EndGame();
            }
            playerScore[k].GetComponent<Image>().color = c[k];
            playerScore[k].transform.GetChild(0).GetComponent<Text>().text = i[k].ToString();
            k = k + 1;
        }

        scoreText.text = _round.ToString();
        _round += 1;
        score.SetActive(true);
    }

    void EndGame()
    {
        foreach (Transform t in Players.transform)
        {
            t.GetComponent<PlayerManager>().enabled = false;
            t.GetComponent<PMenu>()._validate = false;
            t.position = new Vector3(100, 100, 100);
        }
    }

    void SpawnBandage()
    {
        int i = Random.Range(3, 6);
        int j = 0;

        while (j < i)
        {
            GameObject g = bandage[0];
            bandage.RemoveAt(0);
            g.GetComponent<Bandage>().enabled = true;
            g.GetComponent<Bandage>().Reset(1);
            int l = Random.Range(0, 6);
            g.transform.position = GetComponent<GameManagerMenu>().maps[GetComponent<GameManagerMenu>().mapIndex].GetComponent<SpawnPositionData>().bandelette[l];
            j = j + 1;
        }
    }

    void Reset()
    {
        score.SetActive(false);
        foreach (Transform g in Players.transform)
            g.GetComponent<PlayerManager>().Reset();

        bandage.Clear();
        foreach (Transform b in bandages)
        {
            bandage.Add(b.gameObject);
            b.position = new Vector3(100, 100, 100);
        }
        _death = 0;
        GetComponent<GameManagerMenu>().Filtre.SetActive(false);
    }

    void SelectMap()
    {
        GameManagerMenu gmm = GetComponent<GameManagerMenu>();

        int i = Random.Range(0, gmm.maps.Length);

        gmm.maps[gmm.mapIndex].SetActive(false);
        gmm.mapIndex = i;
        gmm.maps[gmm.mapIndex].SetActive(true);
    }
    

}
