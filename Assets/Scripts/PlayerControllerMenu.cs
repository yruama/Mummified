using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControllerMenu : MonoBehaviour
{
    [Header("BlocPersonnage")]
    public GameObject Cadre;
    public Text state;
    public GameObject Mummy;
    public Color[] Color;
    private int _indexColor;

    [Header("Infos")]
    public int playerId;
    public float timeToChangeColor;
    public GameManagerMenu gmm;

    private Color _currentColor;
    private float _timeColor;
    private bool _validate;

	void Start ()
    {
        _validate = false;
        _timeColor = Time.time;
        _indexColor = playerId - 1;
        gmm.availableColors[_indexColor] = -1;
    }
	
	void Update ()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_" + playerId), Input.GetAxisRaw("Vertical_" + playerId));

        if (input.x > 0 && Time.time - _timeColor > timeToChangeColor)
        {
            _timeColor = Time.time;
            while (gmm.availableColors[_indexColor] != 0)
                _indexColor += 1;
            if (_indexColor > gmm.colors.Length - 1)
                _indexColor = 0;
        }
        else if (input.x < 0 && Time.time - _timeColor > timeToChangeColor)
        {
            _timeColor = Time.time;
            while (gmm.availableColors[_indexColor] != 0)
                _indexColor += 1;
            if (_indexColor < 0)
                _indexColor = gmm.colors.Length - 1;
        }

        Cadre.GetComponent<Image>().sprite = gmm.cadre[_indexColor];
    }
}
