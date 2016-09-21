using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControllerMenu : MonoBehaviour
{
    [Header("BlocPersonnage")]
    public Sprite Disable;
    public GameObject Cadre;
    public Sprite[] mummie;
    public Image currentMummie;
    public Text state;
    public GameObject Mummy;
    public Color[] Color;
    private int _indexColor;

    [Header("Infos")]
    public int playerId;
    public float timeToChangeColor;
    public GameManagerMenu gmm;
    public bool _available;

    public Menu m;

    private Color _currentColor;
    private Color _saveColor;
    private float _timeColor;
    private bool _validate;

	void Start ()
    {
        _available = false;
        _validate = false;
        _timeColor = Time.time;
        _indexColor = 0;
    }
	
	void Update ()
    {
        if (m.currentMenu != 2)
            return;
        if (!_available)
            return;
        if (!_validate)
        {
            if (Input.GetButtonDown("Attack_1"))
            {
                m.SetMenu(1);
            }
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
            Cadre.GetComponent<Image>().sprite = gmm.cadre[_indexColor];

            if (Input.GetButtonDown("Jump_" + playerId))
            {
                _validate = true;
                _saveColor = gmm.colors[_indexColor];
                currentMummie.sprite = mummie[2];
                state.text = "ready"; 
                gmm.LockColor(_indexColor);
                gmm.CheckValidate();
            }
        }
        else
        {
            if (Input.GetButtonDown("Attack_" + playerId))
            {
                _validate = false;
                gmm.availableColors[_indexColor] = 0;
                state.text = gmm.colorsName[_indexColor];
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
            state.text = "more than undead";
        }
        else
        {
            _available = true;
        }
    }

    public Color GetColor()
    {
        return gmm.colors[_indexColor];
    }
}
