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
    public bool _end;
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
    public Text decompte;
    private float _decompte;
    public bool _DecompteBool;

    [Header("Score")]
    public GameObject[] playerScore;
    public Text scoreText;
    private int _round;
    public GameObject score;
    public GameObject winner;
    private bool _next;
    public GameObject PressButton;
    Color winnerColor;
    int lastScore;

    [Header("Sound")]
    public AudioClip theme;
    private AudioSource _audio;

    private bool _isPause;
    int nbRound;

    void Start()
    {
        _isPause = false;
        _endGame = false;
        GetComponent<AudioSource>().enabled = true;
        _round = 1;
        _end = false;
        foreach (Transform b in bandages)
            bandage.Add(b.gameObject);
        nbRound = GetComponent<GameManagerMenu>().nbRound;
        _DecompteBool = false;
        _currentManche = 0;
    }

    void Update()
    {
        /* *** DECOMPTE *** */
        if (Time.time - _decompte > 3.0f && _DecompteBool)
        {
            decompte.gameObject.SetActive(false);
            _DecompteBool = false;
            
        }
        /* *** *** */

        if (Time.time - _timeBandage > timeBandageEvent && _end == false)
        {
            _timeBandage = Time.time;
            SpawnBandage();
        }

        if (Time.time - _timeScore > timeToDisplayScore && _end == true && _endGame == false)
        {
            _next = true;
            PressButton.SetActive(true);
        }

        if (_next == true && Input.GetButtonDown("Jump_1") && _end == true && _endGame == false)
        {
            
            score.SetActive(false);
            _DecompteBool = true;
            _end = false;
            decompte.gameObject.SetActive(true);
            _next = false;
            PressButton.SetActive(false);
            Debug.Log("Afficher Score");
            _decompte = Time.time;
            _timeBandage = Time.time;
            SelectMap();
            Reset();
        }

        if (decompte.gameObject.activeSelf == true)
        {
            
            decompte.text = ((int)(4.0f - (Time.time - _decompte))).ToString();
        }

        if (_next == true && _endGame == true && Input.GetButtonDown("Jump_1"))
        {
            Application.LoadLevel("Game");
        }

        if (Time.time - _timeWinner > timeToDisplayWinner && _endGame == true)
        {
            PressButton.SetActive(true);
            _next = true;
             
            //Debug.Log("MDR");
            /* GetComponent<GameManagerMenu>().Filtre.SetActive(true);
             m.SetMenu(1);
             m.MenuGame.SetActive(true);
             score.SetActive(false);
             _endGame = false;
             GetComponent<GameManagerGame>().enabled = false;*/
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

    void BougeHero()
    {
        int i = 0;

        while (i < 4)
        {
            Players.transform.GetChild(i).transform.position = new Vector3(0, 30, 100);
            i = i + 1;
        }
    }

    void DisplayScore()
    {
        BougeHero();
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
       /* while (k < 4)
        {
            if (i[k] >= nbKill)
            {
                GetComponent<AudioSource>().Stop();

              /*  winner.SetActive(true);
                winner.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Players.transform.GetChild(k).GetComponent<PlayerManager>().color;
              
                _timeWinner = Time.time;
                _endGame = true;
                EndGame();
            }
            playerScore[k].GetComponent<Image>().color = c[k];
           /* if (i[k] - Players.transform.GetChild(k).GetComponent<PlayerManager>().ScoreMalus >= 0)
               playerScore[k].transform.GetChild(0).GetComponent<Text>().text = (i[k] - Players.transform.GetChild(k).GetComponent<PlayerManager>().ScoreMalus).ToString();
            else
            {
                playerScore[k].transform.GetChild(0).GetComponent<Text>().text = 0.ToString();
            }*/
            /*SetColor(k);
            SetKill(k);
            SetKillLaser(k);
            SetHit(k);
            SetLaser(k);*
            k = k + 1;
        }/*/

        while (k < 4)
        {
            playerScore[k].GetComponent<Image>().color = c[k];
            int sscore = Players.transform.GetChild(k).GetComponent<PlayerManager>().score;
            if (sscore < 0)
                sscore = 0;
            if (lastScore < sscore)
            {
                lastScore = sscore;
                winnerColor = c[k];
            }

            playerScore[k].transform.GetChild(0).GetComponent<Text>().text = sscore.ToString();
            k = k + 1;
        }

        scoreText.text = _round.ToString();
        _round += 1;
        score.SetActive(true);

        if (_round > nbRound)
        {
            winner.SetActive(true);
            winner.transform.GetChild(0).GetComponent<Image>().color = winnerColor;
            _timeWinner = Time.time;
            _endGame = true;
           // EndGame();
        }
    }

    void SetColor(int i)
    {
        int j = 0;
        int k = 0;
        foreach (Transform t in Players.transform)
        {
            if (j == i)
            {
                j = j + 1;
            }
            if (j >= 4)
            {
                return;
            }
            playerScore[i].transform.GetChild(1 + k).GetComponent<Image>().color = Players.transform.GetChild(j).GetComponent<PlayerManager>().color;

            j = j + 1;
            k = k + 1;
        }
    }

    void SetKill(int nb)
    {
        int[] i = new int[4];
        int[] ii = new int[4];
        int k = 0;
        int j = 0;

        PlayerManager pm = Players.transform.GetChild(nb).GetComponent<PlayerManager>();

        while (j < pm.pHealth.killer.Count)
        {
            if (pm.pHealth.killer[j] == 1)
            {
                ii[0] += 1;
            }
            if (pm.pHealth.killer[j] == 2)
            {
                ii[1] += 1;
            }
            if (pm.pHealth.killer[j] == 3)
            {
                ii[2] += 1;
            }
            if (pm.pHealth.killer[j] == 4)
            {
                ii[3] += 1;
            }
            j = j + 1;
        }

        k = 0;
        j = 0;
        while (k < 3)
        {
            if (k == nb)
                k += 1;
            playerScore[nb].transform.GetChild(4 + j).GetComponent<Text>().text = ii[k].ToString();
            j = j + 1;
            k = k + 1;
        }
    }

    void SetKillLaser(int nb)
    {
        int[] i = new int[4];
        int[] ii = new int[4];
        int k = 0;
        int j = 0;

        PlayerManager pm = Players.transform.GetChild(nb).GetComponent<PlayerManager>();

        while (j < pm.pHealth.killerL.Count)
        {
            if (pm.pHealth.killerL[j] == 1)
            {
                ii[0] += 1;
            }
            if (pm.pHealth.killerL[j] == 2)
            {
                ii[1] += 1;
            }
            if (pm.pHealth.killerL[j] == 3)
            {
                ii[2] += 1;
            }
            if (pm.pHealth.killerL[j] == 4)
            {
                ii[3] += 1;
            }
            j = j + 1;
        }

        k = 0;
        j = 0;
        while (k < 3)
        {
            if (k == nb)
                k += 1;
            playerScore[nb].transform.GetChild(7 + j).GetComponent<Text>().text = ii[k].ToString();
            j = j + 1;
            k = k + 1;
        }
    }

    void SetHit(int nb)
    {
        int[] i = new int[4];
        int[] ii = new int[4];
        int k = 0;
        int j = 0;

        PlayerManager pm = Players.transform.GetChild(nb).GetComponent<PlayerManager>();


        while (j < pm.pHealth.hitterB.Count)
        {
            if (pm.pHealth.hitterB[j] == 1)
            {
                ii[0] += 1;
            }
            if (pm.pHealth.hitterB[j] == 2)
            {
                ii[1] += 1;
            }
            if (pm.pHealth.hitterB[j] == 3)
            {
                ii[2] += 1;
            }
            if (pm.pHealth.hitterB[j] == 4)
            {
                ii[3] += 1;
            }
            j = j + 1;
        }

        k = 0;
        j = 0;
        while (k < 3)
        {
            if (k == nb)
                k += 1;
            playerScore[nb].transform.GetChild(10 + j).GetComponent<Text>().text = ii[k].ToString();
            j = j + 1;
            k = k + 1;
        }
    }

    void SetLaser(int nb)
    {
        int[] i = new int[4];
        int[] ii = new int[4];
        int k = 0;
        int j = 0;

        PlayerManager pm = Players.transform.GetChild(nb).GetComponent<PlayerManager>();

        while (j < pm.pHealth.hitterL.Count)
        {
            if (pm.pHealth.hitterL[j] == 1)
            {
                ii[0] += 1;
            }
            if (pm.pHealth.hitterL[j] == 2)
            {
                ii[1] += 1;
            }
            if (pm.pHealth.hitterL[j] == 3)
            {
                ii[2] += 1;
            }
            if (pm.pHealth.hitterL[j] == 4)
            {
                ii[3] += 1;
            }
            j = j + 1;
        }

        k = 0;
        j = 0;
        while (k < 3)
        {
            if (k == nb)
                k += 1;
           
            playerScore[nb].transform.GetChild(13 + j).GetComponent<Text>().text = ii[k].ToString();
            j = j + 1;
            k = k + 1;
        }
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
        indexEye.Clear();
        gmm.maps[gmm.mapIndex].SetActive(false);
        gmm.mapIndex = i;
        gmm.maps[gmm.mapIndex].SetActive(true);
    }
    

}
