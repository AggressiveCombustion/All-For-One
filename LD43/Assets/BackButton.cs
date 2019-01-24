using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            ClickBack();
        }
	}

    public void ClickBack()
    {
        GameManager.instance.timers.Add(new Timer(3, GoToMenu));
        GameObject.Find("Fade").AddComponent<FadeIn>();
        GameObject.Find("Fade").GetComponent<FadeIn>().fadeSpeed = 1;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MidTitle");
    }
}
