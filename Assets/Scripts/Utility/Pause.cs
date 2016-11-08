using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Pause : MonoBehaviour {

    public float timeToHold;
    public GameObject imagePause;
    public bool pause = false;
    public GameObject menuPause;
    private bool _canPause;
    private float _time;
    public GameObject first;
    public GameObject second;

    void Start()
    {
        _canPause = false;
        pause = false;
    }

	void Update ()
    {
        if (Input.GetButtonDown("Pause") && !pause)
        {
            imagePause.SetActive(true);
            _canPause = true;
            _time = Time.time;
        }

        if (Input.GetButtonDown("Pause") && pause)
        {
            Resume();
        }

        if (Input.GetButtonUp("Pause"))
        {
            _canPause = false;
            imagePause.SetActive(false);
        }
        
        if (_canPause && Time.time - _time > timeToHold)
        {
            GetComponent<GameManagerGame>().m.eventSystem.SetActive(true);
            GetComponent<GameManagerGame>().m.eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = first;
            GetComponent<GameManagerMenu>().Filtre.SetActive(true);
            pause = true;
            Time.timeScale = 0;
            menuPause.SetActive(true);
        }

        if (_canPause)
        {

            imagePause.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2((Time.time - _time) * 100 / timeToHold, 15);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackMenu()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Game");
    }

    public void Resume()
    {
        GetComponent<GameManagerGame>().m.eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = second;
        GetComponent<GameManagerGame>().m.eventSystem.SetActive(false);
        menuPause.SetActive(false);
        pause = false;
        GetComponent<GameManagerMenu>().Filtre.SetActive(false);
        Time.timeScale = 1;
    }
}
