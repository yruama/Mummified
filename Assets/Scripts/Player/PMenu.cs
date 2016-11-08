using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PMenu : MonoBehaviour
{
    private PlayerManager player;

    [Header("Element In Game")]
    public Image cadre;
    public Image mummy;
    public Image controller;
    public Text t;

    [Header("Data")]
    public Sprite[] stateController;
    public Sprite[] mummies;
    public Sprite Disable;

    [Header("Autre")]
    public float timeToChangeColor;
    private float _timeColor;

    [HideInInspector]
    public bool _validate;
    private int _indexColor;
    private Color _currentColor;
    private Color _saveColor;
    private bool _isOk;
    private bool _tresSale;
    private bool _action;

    public AudioClip Sound;
    public AudioClip ValidateSound;

    public Menu m;

    void Start()
    {

        player = GetComponent<PlayerManager>();
        Debug.Log("No Player");

        _validate = false;
        _tresSale = false;
        _isOk = false;
        _timeColor = Time.time;
        _indexColor = 0;

        if (player.playerId == 1)
            _isOk = true;
    }

    void Update()
    {
      /*  while (player == null)
        {
            Debug.Log("coucou");
            player = GetComponent<PlayerManager>();
        }*/
           

        /* *** Menu Back *** */
        if (player.available && !_validate && Input.GetButtonDown("Attack_1") && m.currentMenu != 1)
        {
            if (m.currentMenu == 2 && _action == false)
                return;
            m.SetMenu(1);
        }
        /* *** Menu Back *** */

        /* *** Player DC *** */
        if (!player.available || m.currentMenu != 2)
            return;
        /* *** Player DC END *** */

        
        /* *** Set Up Player *** */
        if (!_isOk)
        {
            if (Input.GetButtonDown("Jump_" + player.playerId))
            {
                _tresSale = true;
                _isOk = true;
            }
            else
            {
                return;
            }
        }
        /* *** Set UP player End *** */
        if (_validate == false)
        {
            if (player.input.x > 0 && Time.time - _timeColor > timeToChangeColor)
            {
                GetComponent<AudioSource>().PlayOneShot(Sound);
                _indexColor += 1;
                _timeColor = Time.time;
                if (_indexColor > player.gmm.colors.Length - 1)
                    _indexColor = 0;
                while (player.gmm.availableColors[_indexColor] != 0)
                    _indexColor += 1;
            }
            else if (player.input.x < 0 && Time.time - _timeColor > timeToChangeColor)
            {
                GetComponent<AudioSource>().PlayOneShot(Sound);
                _indexColor -= 1;
                _timeColor = Time.time;
                if (_indexColor < 0)
                    _indexColor = player.gmm.colors.Length - 1;
                while (player.gmm.availableColors[_indexColor] != 0)
                    _indexColor -= 1;
            }

            t.text = player.gmm.colorsName[_indexColor];
            t.color = player.gmm.colors[_indexColor];
            mummy.GetComponent<RectTransform>().sizeDelta = new Vector2(189, 176);
            mummy.sprite = mummies[0];
            mummy.color = player.gmm.colors[_indexColor];


            if (Input.GetButtonDown("Jump_" + player.playerId) && _tresSale == false)
            {
                GetComponent<AudioSource>().PlayOneShot(ValidateSound);
                cadre.GetComponent<Image>().sprite = player.gmm.cadre[_indexColor];
                _validate = true;
                _saveColor = player.gmm.colors[_indexColor];
                player.color = _saveColor;
                mummy.sprite = mummies[2];
                t.text = "ready";
                player.gmm.LockColor(_indexColor);
                player.gmm.CheckValidate();
            }

            if (Input.GetButtonUp("Jump_" + player.playerId) && _tresSale == true)
            {
                _tresSale = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Attack_" + player.playerId))
            {
                _action = false;
                _validate = false;
                player.gmm.availableColors[_indexColor] = 0;
                t.text = player.gmm.colorsName[_indexColor];
                cadre.GetComponent<Image>().sprite = Disable;
            }
        }

        if (Input.GetButtonUp("Attack_" + player.playerId))
        {
            _action = true;
        }
    }

    public void CheckColor()
    {
        if (_validate)
            return;
        if (player.gmm.availableColors[_indexColor] == -1)
        {
            while (player.gmm.availableColors[_indexColor] == -1)
            {
                _indexColor += 1;
                if (_indexColor > player.gmm.colors.Length - 1)
                    _indexColor = 0;
            }
        }
    }

    public bool GetValidate()
    {
        return _validate;
    }

    public void SetAvailaible(int i)
    {
        if (player == null)
            return;
        if (player.playerId > i)
        {
            player.available = false;
            cadre.GetComponent<Image>().sprite = Disable;
            mummy.sprite = mummies[1];
            mummy.color = new Color(255, 255, 255, 1);
            mummy.GetComponent<RectTransform>().sizeDelta = new Vector2(97, 173);
            t.text = "elsewhere";
            controller.sprite = stateController[1];
            controller.GetComponent<RectTransform>().sizeDelta = new Vector2(130.4f, 80.53333f);
        }
        else
        {
            player.available = true;
            controller.sprite = stateController[0];
            controller.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
        }
    }

    public Color GetColor()
    {
        return player.gmm.colors[_indexColor];
    }
}









    /*
    [Header("BlocPersonnage")]
    public GameObject Cadre;
    public Image currentMummie;
    public Text state;
    public GameObject Mummy;
    public Color[] Color;
    private int _indexColor;
    public Image controller;

    [Header("GameObject")]
    public Sprite[] stateController;
    public Sprite[] mummie;
    public Sprite Disable;

    [Header("Infos")]
    public int playerId;
    public float timeToChangeColor;
    public GameManagerMenu gmm;
    public bool _available;
    private bool _isOk;
    private bool _tresSale;

    public Menu m;

    private Color _currentColor;
    private Color _saveColor;
    private float _timeColor;
    public bool _validate;

    void Start()
    {
        _tresSale = false;
        _isOk = true;
        _available = false;
        _validate = false;
        _timeColor = Time.time;
        _indexColor = 0;
    }

    void Update()
    {
        if (_validate == false && Input.GetButtonDown("Attack_1") && playerId == 1)
        {
            m.SetMenu(1);
        }
        if (m.currentMenu != 2)
            return;
        if (!_available)
            return;
        if (_isOk)
        {
            if (Input.GetButtonDown("Jump_" + playerId))
            {
                _tresSale = true;
                _isOk = false;
            }
            else
            {
                return;
            }
        }
        if (_validate == false)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));
            if (input.x > 0 && Time.time - _timeColor > timeToChangeColor)
            {
                _indexColor += 1;
                _timeColor = Time.time;
                if (_indexColor > gmm.colors.Length - 1)
                    _indexColor = 0;
                while (gmm.availableColors[_indexColor] != 0)
                    _indexColor += 1;
            }
            else if (input.x < 0 && Time.time - _timeColor > timeToChangeColor)
            {
                _indexColor -= 1;
                _timeColor = Time.time;
                if (_indexColor < 0)
                    _indexColor = gmm.colors.Length - 1;
                while (gmm.availableColors[_indexColor] != 0)
                    _indexColor -= 1;
            }

            state.text = gmm.colorsName[_indexColor];
            state.color = gmm.colors[_indexColor];
            currentMummie.GetComponent<RectTransform>().sizeDelta = new Vector2(189, 176);
            currentMummie.sprite = mummie[0];
            currentMummie.color = gmm.colors[_indexColor];


            if (Input.GetButtonDown("Jump_" + playerId) && _tresSale == false)
            {
                Cadre.GetComponent<Image>().sprite = gmm.cadre[_indexColor];
                _validate = true;
                _saveColor = gmm.colors[_indexColor];
                currentMummie.sprite = mummie[2];
                state.text = "ready";
                gmm.LockColor(_indexColor);
                gmm.CheckValidate();
            }

            if (Input.GetButtonUp("Jump_" + playerId) && _tresSale == true)
            {
                _tresSale = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Attack_" + playerId))
            {
                _validate = false;
                gmm.availableColors[_indexColor] = 0;
                state.text = gmm.colorsName[_indexColor];
                Cadre.GetComponent<Image>().sprite = Disable;
            }
        }
    }

    public void CheckColor()
    {
        if (_validate)
            return;
        if (gmm.availableColors[_indexColor] == -1)
        {
            while (gmm.availableColors[_indexColor] == -1)
            {
                _indexColor += 1;
                if (_indexColor > gmm.colors.Length - 1)
                    _indexColor = 0;
            }
        }
    }

    public bool GetValidate()
    {
        return _validate;
    }

    public void SetAvailaible(int i)
    {
        if (playerId > i)
        {
            _available = false;
            Cadre.GetComponent<Image>().sprite = Disable;
            currentMummie.sprite = mummie[1];
            currentMummie.color = new Color(255, 255, 255, 1);
            currentMummie.GetComponent<RectTransform>().sizeDelta = new Vector2(97, 173);
            state.text = "elsewhere";
            controller.sprite = stateController[1];
            controller.GetComponent<RectTransform>().sizeDelta = new Vector2(130.4f, 80.53333f);
        }
        else
        {
            _available = true;
            controller.sprite = stateController[0];
            controller.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
        }
    }

    public Color GetColor()
    {
        return gmm.colors[_indexColor];
    }
}
*/