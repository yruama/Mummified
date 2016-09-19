using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	void Start ()
    {
        foreach (Transform b in bandages)
            bandage.Add(b.gameObject);

        _currentManche = 0;
	}
	
	void Update ()
    {
	
	}

    public void CheckDeath()
    {
        _death += 1;

        if (_death + 1 == nbPlayer)
        {
            Debug.Log("FIN DE LA PARTIE");
        }
    }
}
