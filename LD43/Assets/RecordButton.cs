using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordButton : MonoBehaviour {

    public Transform resSprite;
    public Transform stopSprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        bool isRecording = false;

        foreach(PlayerController p in FindObjectsOfType<PlayerController>())
        {
            if(p.state == PlayerState.recording)
            {
                isRecording = true;
            }
        }

        stopSprite.gameObject.SetActive(isRecording);
	}

    public void ClickRecord()
    {
        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            if (p.state == PlayerState.recording)
            {
                p.StopRecording();
            }

            if (p.state == PlayerState.idle && p.active)
            {
                p.StartRecording();
            }
        }

        if(GameObject.Find("Instructions1") != null)
        {
            GameObject.Find("Instructions1").GetComponent<Instructions1>().DoTheThing();
        }
        
    }
}
