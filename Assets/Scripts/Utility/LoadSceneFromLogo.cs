using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneFromLogo : MonoBehaviour
{
    public float speed;
    public GameObject logo;
    public AudioClip audio;

    private float _alpha;
    private bool _finished;

    private Color _image;
    private Color _text;

	void Start ()
    {
        Cursor.visible = false;
        _image = logo.transform.GetChild(0).GetComponent<Image>().color;
        _text = logo.transform.GetChild(1).GetComponent<Text>().color;
        _finished = true;
    }
	
	void Update ()
    {
	    if (_alpha < 1)
        {
            logo.transform.GetChild(0).GetComponent<Image>().color = new Color(_image.r, _image.g, _image.b, _alpha);
            logo.transform.GetChild(1).GetComponent<Text>().color = new Color(_text.r, _text.g, _text.b, _alpha);
           
        }
        
        if (_finished)
        {
            _finished = false;
            GetComponent<AudioSource>().PlayOneShot(audio);
        }

        if (_alpha >= 1.5f)
        {
            StartGame();
        }

        _alpha += (speed / 100);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
