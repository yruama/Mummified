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

    [Header("Event")]
    public Transform[] BandagePosition;
    public float timeBandageEvent;
    private float _timeBandage;

    void Start ()
    {
        foreach (Transform b in bandages)
            bandage.Add(b.gameObject);

        _currentManche = 0;
	}
	
	void Update ()
    {
	    if (Time.time - _timeBandage > timeBandageEvent)
        {
            _timeBandage = Time.time;
            SpawnBandage();
        }
	}

    public void CheckDeath()
    {
        _death += 1;

        if (_death + 1 == nbPlayer)
        {
            Debug.Log("FIN DE LA PARTIE");
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
            g.transform.position = BandagePosition[l].position;
            j = j + 1;
        }

    }


}
