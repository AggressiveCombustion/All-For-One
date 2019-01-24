using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject cam;

    public float speed = 1.0f;

    public float gravity = 8.0f;

    public List<Timer> timers = new List<Timer>();

    public GameObject explosionPrefab;

    public int elapsed = 0;
    public int limit = 600;

    public bool increaseTime = false;


	// Use this for initialization
	void Start () {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
        increaseTime = false;
    }
	
	// Update is called once per frame
	void Update () {

        foreach (Timer t in timers)
        {
            t.Update();
        }

        if (GameObject.Find("LevelManager") == null)
            return;

        limit = LevelManager.instance.limit;

        if (cam == null)
            cam = GameObject.Find("CameraParent");

        if(increaseTime)
            elapsed += 1;

        if(elapsed >= limit)
            instance.increaseTime = false;

        

        /*for(int i = 0; i < timers.Count; i++)
        {
            if (timers[i].done)
                timers.RemoveAt(i);
        }*/
	}

    public void CameraShake()
    {
        cam.AddComponent<ShakeVertical>();
    }
}

public delegate void TimerEvent();

public class Timer
{
    public float elapsed = 0;
    public float duration = 1;
    public TimerEvent onTimer;
    public bool done = false;

    public Timer(float d, TimerEvent e)
    {
        duration = d;
        onTimer = e;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= duration && !done)
        {
            onTimer();
            done = true;
        }
    }
}
