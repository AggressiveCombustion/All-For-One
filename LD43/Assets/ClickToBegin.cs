using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToBegin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetButtonDown("Fire1"))
        {
            // go to next scene
            gameObject.AddComponent<FadeOut>();
            GetComponent<FadeOut>().colorObject = transform;
            GetComponent<FadeOut>().fadeSpeed = 1;
            GameManager.instance.timers.Add(new Timer(2, GoToTitle));
        }
	}

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
