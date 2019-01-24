using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MidTitle : MonoBehaviour {
    public bool doThing = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
        if(!doThing)
        {
            GameManager.instance.timers.Add(new Timer(1.5f, GoToTitle));
            doThing = true;
        }
	}

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
