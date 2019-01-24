using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {

    bool clicked = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickButton()
    {
        if (clicked)
            return;

        clicked = true;
        GameManager.instance.timers.Add(new Timer(3, GoToLevel));
        GameObject.Find("Fade").AddComponent<FadeIn>();
        GameObject.Find("Fade").GetComponent<FadeIn>().fadeSpeed = 1;
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(name);
    }
}
