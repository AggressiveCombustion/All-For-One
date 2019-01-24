using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    public int numPlayers = 2;

    public int limit = 600;

    public string title = "Let Them Eat Cake";

    public string number = "1-1";

    public float audioElapsed = 0;
    public float audioRate = 0;

	// Use this for initialization
	void Start () {

        if (instance == null)
            instance = this;
        else
            Destroy(this);
        


    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.elapsed < 5)
        {
            audioRate = GameManager.instance.limit / 10;
            audioElapsed = audioRate;
        }

        if (!GameManager.instance.increaseTime || GameManager.instance.elapsed < 5)
            return;

        audioElapsed += 1;// Time.deltaTime;

        if (audioElapsed > audioRate)
        {
            GetComponent<AudioSource>().Play();
            audioElapsed = 0;
            //audioRate = GameManager.instance.limit/ GameManager.instance.elapsed;

            if (GameManager.instance.elapsed > GameManager.instance.limit * 0.3)
            {
                audioRate = (GameManager.instance.limit / 10)/2;
            }

            if (GameManager.instance.elapsed > GameManager.instance.limit * 0.5)
            {
                audioRate = (GameManager.instance.limit / 10) / 3;
            }

            if (GameManager.instance.elapsed > GameManager.instance.limit * 0.75)
            {
                audioRate = (GameManager.instance.limit / 10) / 4;
            }

            if (GameManager.instance.elapsed > GameManager.instance.limit * 0.9)
            {
                audioRate = (GameManager.instance.limit / 10) / 6;
            }
        }
	}
}
